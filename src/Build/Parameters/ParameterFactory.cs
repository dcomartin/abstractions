﻿using System;
using System.Reflection;
using Unity.Attributes;
using Unity.Build.Policy;
using Unity.Container;

namespace Unity.Build.Parameters
{
    public static class ParameterFactory
    {

        public static Factory<Type, ResolvePipeline> ToFactory__(this ParameterInfo parameter, object arg)
        {
            switch (arg)
            {
                case Type type:
                    if (parameter.ParameterType.IsGenericParameter)
                    {
                        var position = parameter.Position;
                        return t =>
                        {
                            var runtimeType = t.GetTypeInfo().GenericTypeArguments[position];

                            if (type == runtimeType)
                                return parameter.ToFactory()(t);

                            return (ref ResolveContext context) => type;
                        };
                    }
                    else if (parameter.ParameterType.GetTypeInfo().IsGenericType)
                    {
                        throw new NotImplementedException();
                    }
                    else if (parameter.ParameterType == type)
                    {
                        return parameter.ToFactory();
                    }
                    else
                        return t => (ref ResolveContext context) => type;


                case ITypeFactory<ParameterInfo> factory:
                    var pipeline = factory.CreatePipeline(parameter);
                    return runtime => pipeline;

                default:
                    return parameter.ToFactory();
            }
        }

        public static Factory<Type, ResolvePipeline> ToFactory(this ParameterInfo parameter, object arg)
        {
            var attribute = (DependencyResolutionAttribute)parameter.GetCustomAttribute(typeof(DependencyResolutionAttribute));
            var info = parameter.ParameterType.GetTypeInfo();

            if (info.IsArray) return parameter.ArrayToFactory(info, attribute);

            if (info.IsGenericType) return parameter.GenericToFactorry(info, attribute);

            if (!info.ContainsGenericParameters)
            {
                // Simple type

                // Factory            // CreatePipeline
                return type => (ref ResolveContext context) => context.Resolve(parameter.ParameterType, attribute?.Name);
            }

            // Parameter of generic type:
            //  private class SomeClass<A, B, C, D>
            //  {
            //      public SomeClass(B b) <--- (B)
            //      ...
            //  }

            var index = info.GenericParameterPosition;

            // Factory            // CreatePipeline
            return type => (ref ResolveContext context) =>
                context.Resolve(type.GetTypeInfo().GenericTypeArguments[index], attribute?.Name);
        }

        /// <summary>
        /// Crates factory pipeline for specific type
        /// </summary>
        /// <param name="parameter">Parameter info to process</param>
        /// <returns>Pipeline factory pipeline</returns>
        public static Factory<Type, ResolvePipeline> ToFactory(this ParameterInfo parameter)
        {
            var attribute = (DependencyResolutionAttribute)parameter.GetCustomAttribute(typeof(DependencyResolutionAttribute));
            var info = parameter.ParameterType.GetTypeInfo();

            if (info.IsArray) return parameter.ArrayToFactory(info, attribute);

            if (info.IsGenericType) return parameter.GenericToFactorry(info, attribute);

            if (!info.ContainsGenericParameters)
            {
                // Simple type

                // Factory            // CreatePipeline
                return type => (ref ResolveContext context) => context.Resolve(parameter.ParameterType, attribute?.Name);
            }

            // Parameter of generic type:
            //  private class SomeClass<A, B, C, D>
            //  {
            //      public SomeClass(B b) <--- (B)
            //      ...
            //  }

            var index = info.GenericParameterPosition;

            // Factory            // CreatePipeline
            return type => (ref ResolveContext context) =>
                context.Resolve(type.GetTypeInfo().GenericTypeArguments[index], attribute?.Name);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Parameter of generic array type:
        ///  private class SomeClass{A, B, C, D}
        ///  {
        ///      public SomeClass(B[]..[] b) --- (B)
        ///      ...
        ///  }
        /// </remarks>
        /// <param name="parameter"></param>
        /// <param name="info"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        private static Factory<Type, ResolvePipeline> ArrayToFactory(this ParameterInfo parameter, 
                                                                                TypeInfo info,
                                                                                DependencyResolutionAttribute attribute)
        {

            var depth = 0;
            var element = parameter.ParameterType;
            while (element != null && element.IsArray)
            {
                depth += 1;
                element = element.GetElementType();
            }

            var elementInfo = element.GetTypeInfo();

            if (elementInfo.IsGenericType)
            {
                // Open generic parameters in @delegate:
                //  {
                //      public SomeClass(DepClass<A, C, int> c) <--- (A, C)
                //  }

                if (!info.ContainsGenericParameters)
                {
                    // All generic parameters are closed:  
                    // ...
                    // SomeClass(DepClass<int, string> c) 

                    // Factory
                    return type =>
                    {
                        var elementType = element;
                        while (0 < depth--)
                        {
                            elementType = elementType?.MakeArrayType() ?? 
                                          throw new InvalidOperationException();
                        }

                        // CreatePipeline
                        return (ref ResolveContext context) => context.Resolve(elementType, attribute?.Name);
                    };
                }
                // Open generic parameters in @delegate:
                //  {
                //      public SomeClass(DepClass<A, C, int> c) <--- (A, C)
                //  }

                var indexes = new int[elementInfo.GenericTypeArguments.Length];
                for (var i = 0; i < indexes.Length; i++)
                {
                    var argument = elementInfo.GenericTypeArguments[i];
                    indexes[i] = argument.IsGenericParameter
                        ? Array.IndexOf(parameter.Member.DeclaringType
                            .GetTypeInfo()
                            .GenericTypeParameters, argument)
                        : -1;
                }

                // Factory
                return type =>
                {
                    var typeInfo = type.GetTypeInfo();
                    var types = new Type[indexes.Length];
                    for (var i = 0; i < indexes.Length; i++)
                    {
                        var index = indexes[i];
                        var newArgType = -1 == index
                            ? elementInfo.GenericTypeArguments[i]
                            : typeInfo.GenericTypeArguments[index];

                        if (newArgType.GetTypeInfo().IsGenericTypeDefinition)
                            throw new InvalidOperationException("Attempting to build open generic");

                        types[i] = newArgType;
                    }

                    var elementType = elementInfo.IsGenericTypeDefinition
                        ? elementInfo.MakeGenericType(types)
                        : elementInfo.GetGenericTypeDefinition().MakeGenericType(types);

                    while (0 < depth--) elementType = elementType.MakeArrayType();

                    // CreatePipeline
                    return (ref ResolveContext context) => context.Resolve(elementType, attribute?.Name);
                };
            }
            if (elementInfo.IsGenericParameter)
            {
                // Parameter of generic type:
                //  private class SomeClass<A, B, C, D>
                //  {
                //      public SomeClass(B b) <--- (B)
                //      ...
                //  }

                int position = element.GenericParameterPosition;

                // Factory
                return type =>
                {
                    var elementType = type.GetTypeInfo().GenericTypeArguments[position];
                    while (0 < depth--) elementType = elementType.MakeArrayType();

                    // CreatePipeline
                    return (ref ResolveContext context) => context.Resolve(elementType, attribute?.Name);
                };
            }

            // Factory
            return runtimeType =>
            {
                var elementType = element;
                while (0 < depth--) elementType = elementType.MakeArrayType();

                // CreatePipeline
                return (ref ResolveContext context) => context.Resolve(elementType, attribute?.Name);
            };
        }

        ///  <summary>
        ///  
        ///  </summary>
        ///  <remarks>
        ///  Member has generic parameters: 
        /// 
        ///   private class SomeClass{A, B, C, D}
        ///       ...
        ///       public SomeClass(DepClass{int, C} c)  --- (C)
        ///  </remarks>
        ///  <param name="parameter"></param>
        ///  <param name="info"></param>
        /// <param name="attribute"></param>
        ///  <returns></returns>
        private static Factory<Type, ResolvePipeline> GenericToFactorry(this ParameterInfo parameter,
                                                                                   TypeInfo info,
                                                                                   DependencyResolutionAttribute attribute)
        {
            if (!info.ContainsGenericParameters)
            {
                // All generic parameters are closed:  
                // ...
                // SomeClass(DepClass<int, string> c) 

                // Factory            // CreatePipeline  
                return type => (ref ResolveContext context) =>
                    context.Resolve(parameter.ParameterType, attribute?.Name);
            }

            // Open generic parameters in @delegate:
            //  {
            //      public SomeClass(DepClass<A, C, int> c) <--- (A, C)
            //  }

            var indexes = new int[info.GenericTypeArguments.Length];
            for (var i = 0; i < indexes.Length; i++)
            {
                var argument = info.GenericTypeArguments[i];
                indexes[i] = argument.IsGenericParameter
                    ? Array.IndexOf(parameter.Member.DeclaringType
                                             .GetTypeInfo()
                                             .GenericTypeParameters, argument)
                    : -1;
            }

            // Factory
            return type =>
            {
                var typeInfo = type.GetTypeInfo();
                var types = new Type[indexes.Length];
                for (var i = 0; i < indexes.Length; i++)
                {
                    var index = indexes[i];
                    var newArgType = -1 == index
                                   ? info.GenericTypeArguments[i]
                                   : typeInfo.GenericTypeArguments[index];

                    if (newArgType.GetTypeInfo().IsGenericTypeDefinition)
                        throw new InvalidOperationException("Attempting to build open generic");

                    types[i] = newArgType;
                }

                var newType = info.IsGenericTypeDefinition
                            ? info.MakeGenericType(types)
                            : info.GetGenericTypeDefinition().MakeGenericType(types);

                // CreatePipeline
                return (ref ResolveContext context) => context.Resolve(newType, attribute?.Name);
            };
        }
    }
}
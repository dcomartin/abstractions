using System;
using System.Reflection;
using Unity.Attributes;
using Unity.Build.Context;
using Unity.Build.Factory;
using Unity.Build.Pipeline;
using Unity.Build.Policy;

namespace Unity.Build.Injection
{
    public static class InjectionParameterInfo
    {

        public static PipelineFactory<Type, ResolveMethod> ToFactory(this ParameterInfo parameter, object arg)
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

                            return (ref ResolutionContext context) => type;
                        };
                    }
                    else if (parameter.ParameterType == type)
                    {
                        return parameter.ToFactory();
                    }
                    else
                        return t => (ref ResolutionContext context) => type;


                case IResolve<ParameterInfo> factory:
                    var pipeline = factory.Resolver(parameter);
                    return runtime => pipeline;

                default:
                    return parameter.ToFactory();
            }
        }

        /// <summary>
        /// Crates factory method for specific type
        /// </summary>
        /// <param name="parameter">Parameter info to process</param>
        /// <returns>Pipeline factory method</returns>
        public static PipelineFactory<Type, ResolveMethod> ToFactory(this ParameterInfo parameter)
        {
            var attribute = (DependencyResolutionAttribute)parameter.GetCustomAttribute(typeof(DependencyResolutionAttribute));
            var info = parameter.ParameterType.GetTypeInfo();

            if (info.IsArray) return parameter.ArrayToFactory(info, attribute);

            if (info.IsGenericType) return parameter.GenericToFactorry(info, attribute);

            if (!info.ContainsGenericParameters)
            {
                // Simple type

                // Factory            // Resolver
                return type => (ref ResolutionContext context) => context.Resolve(parameter.ParameterType, attribute?.Name);
            }

            // Parameter of generic type:
            //  private class SomeClass<A, B, C, D>
            //  {
            //      public SomeClass(B b) <--- (B)
            //      ...
            //  }

            var index = info.GenericParameterPosition;

            // Factory            // Resolver
            return type => (ref ResolutionContext context) =>
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
        private static PipelineFactory<Type, ResolveMethod> ArrayToFactory(this ParameterInfo parameter, 
                                                                                TypeInfo info,
                                                                                DependencyResolutionAttribute attribute)
        {

            var depth = 0;
            var element = parameter.ParameterType;
            while (element.IsArray)
            {
                depth += 1;
                element = element.GetElementType();
            }

            var elementInfo = element.GetTypeInfo();

            if (elementInfo.IsGenericType)
            {
                // Open generic parameters in dependency:
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
                        while (0 < depth--) elementType = elementType.MakeArrayType();

                        // Resolver
                        return (ref ResolutionContext context) => context.Resolve(elementType, attribute?.Name);
                    };
                }
                // Open generic parameters in dependency:
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

                    // Resolver
                    return (ref ResolutionContext context) => context.Resolve(elementType, attribute?.Name);
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

                    // Resolver
                    return (ref ResolutionContext context) => context.Resolve(elementType, attribute?.Name);
                };
            }

            // Factory
            return runtimeType =>
            {
                var elementType = element;
                while (0 < depth--) elementType = elementType.MakeArrayType();

                // Resolver
                return (ref ResolutionContext context) => context.Resolve(elementType, attribute?.Name);
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
        private static PipelineFactory<Type, ResolveMethod> GenericToFactorry(this ParameterInfo parameter,
                                                                                   TypeInfo info,
                                                                                   DependencyResolutionAttribute attribute)
        {
            if (!info.ContainsGenericParameters)
            {
                // All generic parameters are closed:  
                // ...
                // SomeClass(DepClass<int, string> c) 

                // Factory            // Resolver  
                return type => (ref ResolutionContext context) =>
                    context.Resolve(parameter.ParameterType, attribute?.Name);
            }

            // Open generic parameters in dependency:
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

                // Resolver
                return (ref ResolutionContext context) => context.Resolve(newType, attribute?.Name);
            };
        }
    }
}

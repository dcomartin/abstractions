using System;
using System.Reflection;
using Unity.Attributes;
using Unity.Build.Context;
using Unity.Build.Pipeline;
using Unity.Pipeline;

namespace Unity.Utility
{
    public static class ParameterInfoExtensions
    {

        public static CreateResolver<Type> ToFactory(this ParameterInfo parameter, object arg)
        {
            switch (arg)
            {
                case Type type:
                    return parameter.ParameterType.Equals(type)
                           ? parameter.ToFactory()
                           : (Type runtime) => (ref ResolutionContext context) => type;

                case ICreateResolver<ParameterInfo> factory:
                    var pipeline = factory.CreateResolver(parameter);
                    return (Type runtime) => pipeline;

                default:
                    return parameter.ToFactory();
            }
        }

        /// <summary>
        /// Crates factory method for specific type
        /// </summary>
        /// <param name="parameter">Parameter info to process</param>
        /// <returns>Pipeline facotory method</returns>
        public static CreateResolver<Type> ToFactory(this ParameterInfo parameter)
        {
            var attribute = (DependencyResolutionAttribute)parameter.GetCustomAttribute(typeof(DependencyResolutionAttribute));
            var info = parameter.ParameterType.GetTypeInfo();

            if (info.IsArray) return parameter.ArrayToFactory(info, attribute);

            if (info.IsGenericType) return parameter.GenericToFactorry(info, attribute);

            if (!info.ContainsGenericParameters)
            {
                // Simple type

                // Factory            // Resolver
                return (Type type) => (ref ResolutionContext context) => context.Resolve(parameter.ParameterType, attribute?.Name);
            }

            // Parameter of generic type:
            //  private class SomeClass<A, B, C, D>
            //  {
            //      public SomeClass(B b) <--- (B)
            //      ...
            //  }

            var index = info.GenericParameterPosition;

            // Factory            // Resolver
            return (Type type) => (ref ResolutionContext context) =>
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
        /// <param name="declaringType"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        private static CreateResolver<Type> ArrayToFactory(this ParameterInfo parameter,
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
                    return (Type type) =>
                    {
                        var elementType = element;
                        while (0 < depth--) elementType = elementType.MakeArrayType();

                        // Resolver
                        return (ref ResolutionContext context) => context.Resolve(elementType, attribute?.Name);
                    };
                }
                else
                {
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
                    return (Type type) =>
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
                return (Type type) =>
                {
                    var elementType = type.GetTypeInfo().GenericTypeArguments[position];
                    while (0 < depth--) elementType = elementType.MakeArrayType();

                    // Resolver
                    return (ref ResolutionContext context) => context.Resolve(elementType, attribute?.Name);
                };
            }
            else
            {
                // Factory
                return (Type runtimeType) =>
                {
                    var elementType = element;
                    while (0 < depth--) elementType = elementType.MakeArrayType();

                    // Resolver
                    return (ref ResolutionContext context) => context.Resolve(elementType, attribute?.Name);
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Member has generic parameters: 
        ///
        ///  private class SomeClass{A, B, C, D}
        ///      ...
        ///      public SomeClass(DepClass{int, C} c)  --- (C)
        /// </remarks>
        /// <param name="parameter"></param>
        /// <param name="info"></param>
        /// <param name="declaringType"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        private static CreateResolver<Type> GenericToFactorry(this ParameterInfo parameter,
                                                                   TypeInfo info,
                                                                   DependencyResolutionAttribute attribute)
        {
            if (!info.ContainsGenericParameters)
            {
                // All generic parameters are closed:  
                // ...
                // SomeClass(DepClass<int, string> c) 

                // Factory            // Resolver  
                return (Type type) => (ref ResolutionContext context) =>
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
            return (Type type) =>
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

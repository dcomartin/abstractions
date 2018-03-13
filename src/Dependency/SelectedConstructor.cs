using System;
using System.Diagnostics;
using System.Reflection;
using Unity.Attributes;
using Unity.Registration;
using Unity.Resolution;

namespace Unity.Dependency
{
    /// <summary>
    /// Objects of this type are the return value from 
    /// <see cref="Unity.Policy.IConstructorSelectorPolicy.SelectConstructor"/>.
    /// It encapsulates the desired <see cref="ConstructorInfo"/> with the string keys
    /// needed to look up the <see cref="IResolverPolicy"/> for each
    /// parameter.
    /// </summary>
    public class SelectedConstructor : SelectedMemberWithParameters<ConstructorInfo>
    {
        /// <summary>
        /// Create a new <see cref="SelectedConstructor"/> instance which
        /// contains the given constructor.
        /// </summary>
        /// <param name="constructor">The constructor to wrap.</param>
        public SelectedConstructor(ConstructorInfo constructor)
            : base(constructor)
        {
            var parameters = constructor.GetParameters();
            for (var i = 0; i < parameters.Length; i++)
            {
                AddParameterResolver(GetResolvePipeline(constructor, parameters[i]));
            }
        }

        /// <summary>
        /// Create a new <see cref="SelectedConstructor"/> instance which
        /// contains the given constructor.
        /// </summary>
        /// <param name="constructor">The constructor to wrap.</param>
        public SelectedConstructor(ConstructorInfo constructor, InjectionParameter[] values)
            : base(constructor)
        {
            var parameters = constructor.GetParameters();
            Debug.Assert(values.Length == parameters.Length);

            for (var i = 0; i < parameters.Length; i++)
            {
                switch (values[i])
                {
                    case InjectionParameter injectionParameter:
//                        AddParameterResolver((ref ResolutionContext context) => injectionParameter.Value);
                        break;

                    //case ResolvedParameter resolvedParameter:
                    //    //AddParameterResolver((ref ResolutionContext context) => context.Resolve(resolvedParameter.ParameterType, 
                    //    //                                                                        resolvedParameter.ParameterName));
                    //    break;

                    default:
                        AddParameterResolver(GetResolvePipeline(constructor, parameters[i]));
                        break;
                }
            }
        }

        /// <summary>
        /// The constructor this object wraps.
        /// </summary>
        public ConstructorInfo Constructor => MemberInfo;


        #region Implementation

        private ResolvePipeline GetResolvePipeline(ConstructorInfo ctor, ParameterInfo parameter)
        {
            var attribute = (DependencyResolutionAttribute)parameter.GetCustomAttribute(typeof(DependencyResolutionAttribute));
            var info = parameter.ParameterType.GetTypeInfo();

            if (info.IsGenericType && info.ContainsGenericParameters)
            {
                return GenerateGenericResolver(ctor, parameter, attribute?.Name);
            }
            else if (info.IsArray && parameter.ParameterType.GetElementType().GetTypeInfo().IsGenericParameter)
            {
                return GenerateArrayResolver(ctor, parameter, attribute?.Name);
            }

            return (ref ResolutionContext context) => context.Resolve(parameter.ParameterType, attribute?.Name);
        }

        private ResolvePipeline GenerateArrayResolver(ConstructorInfo ctor, ParameterInfo parameter, string name)
        {
            var ctorInfo = ctor.DeclaringType.GetTypeInfo();
            var index = parameter.ParameterType.GetElementType().GenericParameterPosition;

            return (ref ResolutionContext context) =>
            {
                return context.Resolve(ctorInfo.GenericTypeParameters[index].MakeArrayType(), name);
            };
        }

        private ResolvePipeline GenerateGenericResolver(ConstructorInfo ctor, ParameterInfo parameter, string name)
        {
            var paramType = parameter.ParameterType;
            var paramInfo = paramType.GetTypeInfo();
            var ctorInfo = ctor.DeclaringType.GetTypeInfo();

            var indexes = new int[paramInfo.GenericTypeArguments.Length];
            for(var i = 0; i < indexes.Length; i++)
                indexes[i] = paramInfo.GenericTypeArguments[i].GenericParameterPosition;

            return (ref ResolutionContext context) => 
            {
                var types = new Type[indexes.Length];
                for (var i = 0; i < indexes.Length; i++)
                    types[i] = ctorInfo.GenericTypeParameters[indexes[i]];

                return context.Resolve(paramType.MakeGenericType(types), name);
            };
        }

        #endregion
    }
}

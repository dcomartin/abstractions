using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Unity.Builder.Policy;
using Unity.Dependency;
using Unity.Policy;

namespace Unity.Registration
{
    /// <summary>
    /// An <see cref="InjectionMember"/> that configures the
    /// container to call a method as part of buildup.
    /// </summary>
    public class InjectionMethod : InjectionMemberWithParameters
    {
        private readonly string _methodName;

        /// <summary>
        /// Create a new <see cref="InjectionMethod"/> instance which will configure
        /// the container to call the given methods with the given parameters.
        /// </summary>
        /// <param name="methodName">Name of the method to call.</param>
        /// <param name="methodParameters">Parameter values for the method.</param>
        public InjectionMethod(string methodName, params object[] methodParameters)
            : base(methodParameters)
        {
            _methodName = methodName;
        }

        public override void AddPolicies(Type serviceType, Type implementationType, string name, IPolicyList policies)
        {
            //MethodInfo methodInfo = FindMethod(implementationType);
            //ValidateMethodCanBeInjected(methodInfo, implementationType);

            //SpecifiedMethodsSelectorPolicy selector =
            //    GetSelectorPolicy(policies, serviceType, name);
            //selector.AddMethodAndParameters(methodInfo, _methodParameters);
        }

        public override bool BuildRequired => true;

        /// <summary>
        /// A small function to handle name matching. You can override this
        /// to do things like case insensitive comparisons.
        /// </summary>
        /// <param name="targetMethod">MethodInfo for the method you're checking.</param>
        /// <param name="nameToMatch">Name of the method you're looking for.</param>
        /// <returns>True if a match, false if not.</returns>
        protected virtual bool MethodNameMatches(MemberInfo targetMethod, string nameToMatch)
        {
            return (targetMethod ?? throw new ArgumentNullException(nameof(targetMethod))).Name == nameToMatch;
        }

        private MethodInfo FindMethod(Type typeToCreate)
        {
            //foreach (MethodInfo method in typeToCreate.GetMethodsHierarchical())
            //{
            //    if (MethodNameMatches(method, _methodName))
            //    {
            //        if (_methodParameters.Matches(method.GetParameters().Select(p => p.ParameterType)))
            //        {
            //            return method;
            //        }
            //    }
            //}
            return null;
        }

        private void ValidateMethodCanBeInjected(MethodInfo method, Type typeToCreate)
        {
            GuardMethodNotNull(method, typeToCreate);
            GuardMethodNotStatic(method, typeToCreate);
            GuardMethodNotGeneric(method, typeToCreate);
            GuardMethodHasNoOutParams(method, typeToCreate);
            GuardMethodHasNoRefParams(method, typeToCreate);
        }

        private void GuardMethodNotNull(MethodInfo info, Type typeToCreate)
        {
            if (info == null)
            {
                ThrowIllegalInjectionMethod(Constants.NoSuchMethod, typeToCreate);
            }
        }

        private void GuardMethodNotStatic(MethodInfo info, Type typeToCreate)
        {
            if (info.IsStatic)
            {
                ThrowIllegalInjectionMethod(Constants.CannotInjectStaticMethod, typeToCreate);
            }
        }

        private void GuardMethodNotGeneric(MethodInfo info, Type typeToCreate)
        {
            if (info.IsGenericMethodDefinition)
            {
                ThrowIllegalInjectionMethod(Constants.CannotInjectGenericMethod, typeToCreate);
            }
        }

        private void GuardMethodHasNoOutParams(MethodInfo info, Type typeToCreate)
        {
            if (info.GetParameters().Any(param => param.IsOut))
            {
                ThrowIllegalInjectionMethod(Constants.CannotInjectMethodWithOutParams, typeToCreate);
            }
        }

        private void GuardMethodHasNoRefParams(MethodInfo info, Type typeToCreate)
        {
            if (info.GetParameters().Any(param => param.ParameterType.IsByRef))
            {
                ThrowIllegalInjectionMethod(Constants.CannotInjectMethodWithRefParams, typeToCreate);
            }
        }

        private void ThrowIllegalInjectionMethod(string message, Type typeToCreate)
        {
            // TODO:
            //throw new InvalidOperationException(
            //    string.Format(CultureInfo.CurrentCulture,
            //        message,
            //        typeToCreate.GetTypeInfo().Name,
            //        _methodName,
            //        string.Join(", ", Parameters.Select(mp => mp.ParameterType?.Name))));
        }

        private static SpecifiedMethodsSelectorPolicy GetSelectorPolicy(IPolicyList policies, Type typeToCreate, string name)
        {
            var selector = policies.Get(typeToCreate, name, typeof(IMethodSelectorPolicy), out _);
            if (!(selector is SpecifiedMethodsSelectorPolicy))
            {
                selector = new SpecifiedMethodsSelectorPolicy();
                policies.Set(typeToCreate, name, typeof(IMethodSelectorPolicy), selector);
            }
            return (SpecifiedMethodsSelectorPolicy)selector;
        }
    }
}

using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Unity.Build.Context;
using Unity.Build.Factory;
using Unity.Build.Injection;
using Unity.Policy;

namespace Unity.Registration
{
    /// <summary>
    /// A class that holds the collection of information
    /// for a constructor, so that the container can
    /// be configured to call this constructor.
    /// </summary>
    public class InjectionConstructor : InjectionMemberWithParameters<ConstructorInfo>, 
                                        IInjectionConstructor
    {
        #region Constructors

        /// <summary>
        /// Create a new instance of <see cref="InjectionConstructor"/> that looks
        /// for a default constructor.
        /// </summary>
        public InjectionConstructor()
        {
        }

        /// <summary>
        /// Create a new instance of <see cref="InjectionConstructor"/> that looks
        /// for a constructor with the given set of parameters.
        /// </summary>
        /// <param name="args">The args for the parameters, that will
        /// be converted to <see cref="InjectionParameterValue"/> objects.</param>
        public InjectionConstructor(params object[] args)
            : base(args)
        {
        }

        public InjectionConstructor(ConstructorInfo info)
            : base(info)
        {
        }

        #endregion


        #region Register Pipeline

        public override void AddPolicies(Type registeredType, string name, Type implementationType, IPolicySet policies)
        {
            var type = implementationType ?? registeredType;

            foreach (var ctor in type.GetTypeInfo().DeclaredConstructors)
            {
                if (ctor.IsStatic || !ctor.IsPublic || !Matches(ctor.GetParameters()))
                    continue;

                if (null != MemberInfo)
                {
                    throw new InvalidOperationException(ErrorMessage(type,
                        $"The type {{0}} has multiple constructors {MemberInfo}, {ctor}, etc. satisfying signature ( {{1}} ). Unable to disambiguate."));
                }

                MemberInfo = ctor;
            }

            if (null == MemberInfo)
                throw new InvalidOperationException(ErrorMessage(type, Constants.NoSuchConstructor));

            policies.Set(typeof(IInjectionConstructor), this);
        }

        #endregion


        #region IInjectionConstructor

        public ConstructorInfo Constructor => MemberInfo;

        public override ResolveMethodFactory<Type> ResolveMethodFactory => type =>
        {

            var pipeline = base.ResolveMethodFactory(type);

            if (!MemberInfo.DeclaringType.GetTypeInfo().IsGenericTypeDefinition)
            {
                var constructorInfo = MemberInfo;
                return (ref ResolutionContext context) => constructorInfo.Invoke((object[])pipeline(ref context));
            }

            Debug.Assert(MemberInfo.DeclaringType.GetTypeInfo().GetGenericTypeDefinition() == type.GetTypeInfo().GetGenericTypeDefinition());
            // TODO: optimize for already selected

            var index = -1;
            foreach (var member in MemberInfo.DeclaringType.GetTypeInfo().DeclaredConstructors)
            {
                index += 1;
                if (MemberInfo != member) continue;
                break;
            }

            var ctor = type.GetTypeInfo().DeclaredConstructors.ElementAt(index);
            return (ref ResolutionContext context) => ctor.Invoke((object[])pipeline(ref context));
        };

        #endregion
    }
}

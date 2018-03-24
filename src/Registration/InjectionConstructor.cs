using System;
using System.Reflection;
using Unity.Build.Context;
using Unity.Build.Factory;
using Unity.Build.Injection;
using Unity.Build.Pipeline;
using Unity.Build.Policy;
using Unity.Storage;

namespace Unity.Registration
{
    /// <summary>
    /// A class that holds the collection of information
    /// for a constructor, so that the container can
    /// be configured to call this constructor.
    /// </summary>
    public class InjectionConstructor : InjectionMemberWithParameters<ConstructorInfo>, 
                                        IRequireBuild
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
        /// <param name="args">The arguments for the parameters, that will
        /// be converted to <see cref="InjectionParameterValue"/> objects.</param>
        public InjectionConstructor(params object[] args)
            : base(args)
        {
        }

        public InjectionConstructor(ConstructorInfo info)
        {
            MemberInfo = info;
        }

        #endregion


        #region Add Policy

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

            policies.Set(typeof(InjectionConstructor), this);
        }

        #endregion


        #region InjectionMemberWithParameters

        public ConstructorInfo Constructor => MemberInfo;

        protected override PipelineFactory<Type, ResolveMethod> CreateResolverFactory()
        {
            var dependencies = base.CreateResolverFactory();
            if (null == dependencies)
                return type => (ref ResolutionContext context) => MemberInfo.Invoke(null);
            else
                return type =>
                {
                    var resolver = dependencies(type);
                    return (ref ResolutionContext context) => MemberInfo.Invoke((object[])resolver(ref context));
                };
        }

        #endregion
    }
}

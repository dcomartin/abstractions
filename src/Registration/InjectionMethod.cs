using System;
using System.Reflection;
using Unity.Build.Parameters;
using Unity.Storage;

namespace Unity.Registration
{
    /// <summary>
    /// An <see cref="InjectionMember"/> that configures the
    /// container to call a pipeline as part of buildup.
    /// </summary>
    public class InjectionMethod : InjectionMemberWithParameters<MethodInfo>
    {
        #region Fields


        #endregion


        #region Constructors

        public InjectionMethod()
        {
        }

        public InjectionMethod(string name)
        {
        }

        public InjectionMethod(MethodInfo info)
        {
            MemberInfo = info;
        }

        public InjectionMethod(params object[] args)
            : base(args)
        {
        }

        public InjectionMethod(string name, params object[] args)
            : base(args)
        {
        }


        #endregion


        #region IInjectionMethod

        public MethodInfo Method => MemberInfo;

        #endregion

        public override void AddPolicies(Type registeredType, string name, Type implementationType, IPolicySet set)
        {
            throw new NotImplementedException();
        }
    }
}

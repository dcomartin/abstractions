using System.Reflection;
using Unity.Build.Injection;

namespace Unity.Registration
{
    /// <summary>
    /// An <see cref="InjectionMember"/> that configures the
    /// container to call a method as part of buildup.
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
    }
}

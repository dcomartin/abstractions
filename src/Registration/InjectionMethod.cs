using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Dependency;

namespace Unity.Registration
{
    /// <summary>
    /// An <see cref="InjectionMember"/> that configures the
    /// container to call a method as part of buildup.
    /// </summary>
    public class InjectionMethod : InjectionMemberWithParameters<MethodInfo>
    {
        #region Error Constants

        protected override string NoMemberFound => Constants.NoSuchMethod;
        // TODO: App proper error message
        protected override string MultipleFound { get; }

        #endregion

        
        #region Fields

        private readonly string _methodName;

        #endregion


        #region Constructors

        public InjectionMethod()
            : base()
        {
        }

        public InjectionMethod(string name)
            : base()
        {
        }

        public InjectionMethod(Type func)
            : base()
        {
        }

        public InjectionMethod(MethodInfo info)
        {
            MemberInfo = info;
        }

        public InjectionMethod(params object[] methodParameters)
            : base(methodParameters)
        {
        }

        public InjectionMethod(string name, params object[] methodParameters)
            : base()
        {
        }

        #endregion


        #region InjectionMember

        #endregion


        #region Implementation

        protected override IEnumerable<MethodInfo> GetMemberInfos(Type type) => type.GetTypeInfo()
                                                                                    .DeclaredMethods
                                                                                    .Where(m => !m.IsStatic && m.IsPublic);
        protected override ParameterInfo[] GetParameters(MethodInfo methodInfo) => methodInfo.GetParameters();

        #endregion

    }
}

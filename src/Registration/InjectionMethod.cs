using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Unity.Build.Context;
using Unity.Build.Factory;
using Unity.Build.Injection;

namespace Unity.Registration
{
    /// <summary>
    /// An <see cref="InjectionMember"/> that configures the
    /// container to call a method as part of buildup.
    /// </summary>
    public class InjectionMethod : InjectionMemberWithParameters<MethodInfo>, 
                                   IInjectionMethod
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


        public override ResolveMethodFactory<Type> ResolveMethodFactory => (type) =>
        {
            var pipeline = base.ResolveMethodFactory(type);

            if (!MemberInfo.DeclaringType.GetTypeInfo().IsGenericTypeDefinition)
            {
                var methodInfo = MemberInfo;
                return (ref ResolutionContext context) => methodInfo.Invoke(context.Existing, (object[])pipeline(ref context));
            }

            Debug.Assert(MemberInfo.DeclaringType.GetTypeInfo().GetGenericTypeDefinition() == type.GetTypeInfo().GetGenericTypeDefinition());

            // TODO: Check if create info from Generic Type Definition is faster
            var index = -1;
            foreach (var member in MemberInfo.DeclaringType.GetTypeInfo().DeclaredMethods)
            {
                index += 1;
                if (MemberInfo != member) continue;
                break;
            }

            var method = type.GetTypeInfo().DeclaredMethods.ElementAt(index);
            return (ref ResolutionContext context) => method.Invoke(context.Existing, (object[])pipeline(ref context));
        };

        #endregion
    }
}

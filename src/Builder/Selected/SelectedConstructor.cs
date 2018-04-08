using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Unity.Build.Context;
using Unity.Build.Pipeline;
using Unity.Builder.Selected;

namespace Unity.Build.Selected
{
    /// <summary>
    /// This is a wrapper around selected constructor.
    /// </summary>
    public class SelectedConstructor : SelectedMemberWithParameters<ConstructorInfo>
    {
        /// <summary>
        /// Create a new <see cref="SelectedConstructor"/> instance which
        /// contains the given constructor.
        /// </summary>
        /// <param name="constructor">The constructor to wrap.</param>
        public SelectedConstructor(ConstructorInfo constructor)
            : base(constructor, constructor.GetParameters())
        {
        }

        /// <summary>
        /// Create a new <see cref="SelectedConstructor"/> instance which
        /// contains the given constructor.
        /// </summary>
        /// <param name="constructor">The constructor to wrap.</param>
        /// <param name="values"></param>
        public SelectedConstructor(ConstructorInfo constructor, object[] values = null)
            : base(constructor, constructor.GetParameters(), values)
        {
        }


        /// <summary>
        /// The constructor this object wraps.
        /// </summary>
        public ConstructorInfo Constructor => MemberInfo;


        public override Factory<Type, ResolveMethod> ResolveMethodFactory => (Type type) => 
        {
            var pipeline = base.ResolveMethodFactory(type);

            if (!MemberInfo.DeclaringType.GetTypeInfo().IsGenericTypeDefinition)
            {
                var constructorInfo = MemberInfo;
                return (ref ResolutionContext context) => constructorInfo.Invoke((object[])pipeline(ref context));
            }

            Debug.Assert(MemberInfo.DeclaringType.GetTypeInfo().GetGenericTypeDefinition() == type.GetTypeInfo().GetGenericTypeDefinition());

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
    }
}

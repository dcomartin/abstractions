using System;
using System.Reflection;
using Unity.Build.Context;
using Unity.Build.Pipeline;

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


        public override CreateResolver<Type> CreateResolver => (type) => 
        {
            var pipeline = base.CreateResolver(type);
            if (MemberInfo.DeclaringType.GetTypeInfo().IsGenericTypeDefinition)
                return (ref ResolutionContext context) => Activator.CreateInstance(type, (object[])pipeline(ref context));

            return (ref ResolutionContext context) => MemberInfo.Invoke((object[])pipeline(ref context));
        };
    }
}

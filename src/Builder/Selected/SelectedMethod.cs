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
    /// Objects of this type are the return value from <see cref="IMethodSelectorPolicy.SelectMethods"/>.
    /// It encapsulates the desired <see cref="MethodInfo"/> with the string keys
    /// needed to look up the <see cref="IResolverPolicy"/> for each
    /// parameter.
    /// </summary>
    public class SelectedMethod : SelectedMemberWithParameters<MethodInfo>
    {
        /// <summary>
        /// Create a new <see cref="SelectedMethod"/> instance which
        /// contains the given method.
        /// </summary>
        /// <param name="method">The method</param>
        public SelectedMethod(MethodInfo method)
            : base(method, method.GetParameters())             
        {
        }


        /// <summary>
        /// The constructor this object wraps.
        /// </summary>
        public MethodInfo Method => MemberInfo;


        public override PipelineFactory<Type, ResolveMethod> ResolveMethodFactory => (type) =>
        {
            // TODO: Fix
            throw new NotImplementedException();
            //var pipeline = base.ResolveMethodFactory(type);

            //if (!MemberInfo.DeclaringType.GetTypeInfo().IsGenericTypeDefinition)
            //{
            //    var methodInfo = MemberInfo;
            //    return (ref ResolutionContext context) => methodInfo.Invoke(context.Existing, (object[])pipeline(ref context));
            //}

            //Debug.Assert(MemberInfo.DeclaringType.GetTypeInfo().GetGenericTypeDefinition() == type.GetTypeInfo().GetGenericTypeDefinition());

            //// TODO: Check if create info from Generic Type Definition is faster
            //var index = -1;
            //foreach (var member in MemberInfo.DeclaringType.GetTypeInfo().DeclaredMethods)
            //{
            //    index += 1;
            //    if (MemberInfo != member) continue;
            //    break;
            //}

            //var method = type.GetTypeInfo().DeclaredMethods.ElementAt(index);
            //return (ref ResolutionContext context) => method.Invoke(context.Existing, (object[])pipeline(ref context));
        };
    }
}

using System;
using System.Reflection;
using Unity.Container;

namespace Unity.Builder.Selected
{
    /// <summary>
    /// Base class for return of selector policies that need
    /// to keep track of a set of parameter resolvers.
    /// </summary>
    public class SelectedMemberWithParameters 
    {
        #region Constructors

        protected SelectedMemberWithParameters(ParameterInfo[] parameters)
        {
            //var length = parameters.Length;
            //if (0 == length)
            //{
            //    var array = new object[0];
            //    ResolveMethodFactory = type => (ref ResolveContext context) => array;
            //}
            //else
            //{
            //    var factories = new Factory<Type, ResolvePipeline>[length];

            //    for (var f = 0; f < length; f++) factories[f] = parameters[f].ToFactory();

            //    ResolveMethodFactory = type =>
            //    {
            //        var resolvers = new ResolvePipeline[length];
            //        for (var p = 0; p < length; p++) resolvers[p] = factories[p](type);

            //        return (ref ResolveContext context) =>
            //        {
            //            var values = new object[length];
            //            for (var v = 0; v < length; v++) values[v] = resolvers[v](ref context);
            //            return values;
            //        };
            //    };
            //}
        }

        protected SelectedMemberWithParameters(ParameterInfo[] parameters, object[] members)
        {
            //var length = parameters.Length;
            //if (0 == length)
            //{
            //    var array = new object[0];
            //    ResolveMethodFactory = type => (ref ResolveContext context) => array;
            //}
            //else
            //{
            //    var factories = new Factory<Type, ResolvePipeline>[length];

            //    if (null == members || 0 == members.Length)
            //        for (var f = 0; f < length; f++) factories[f] = parameters[f].ToFactory();
            //    else
            //    {
            //        Debug.Assert(length == members.Length, "Number of InjectionMembers and parameters are different.");
            //        for (var f = 0; f < length; f++) factories[f] = parameters[f].ToFactory(members[f]);
            //    }

            //    ResolveMethodFactory = type =>
            //    {
            //        var resolvers = new ResolvePipeline[length];
            //        for (var p = 0; p < length; p++) resolvers[p] = factories[p](type);

            //        return (ref ResolveContext context) =>
            //        {
            //            var values = new object[length];
            //            for (var v = 0; v < length; v++) values[v] = resolvers[v](ref context);
            //            return values;
            //        };
            //    };
            //}
        }

        #endregion


        #region Factory

        public virtual Factory<Type, ResolvePipeline> ResolveMethodFactory { get; }

        #endregion
    }


    public class SelectedMemberWithParameters<TMemberInfoType> : SelectedMemberWithParameters
    {
        protected SelectedMemberWithParameters(TMemberInfoType memberInfo, ParameterInfo[] parameters)
            : base(parameters)
        {
            MemberInfo = memberInfo;
        }

        protected SelectedMemberWithParameters(TMemberInfoType memberInfo, ParameterInfo[] parameters, object[] members)
            : base(parameters, members)
        {
            MemberInfo = memberInfo;
        }

        /// <summary>
        /// The member info stored.
        /// </summary>
        protected TMemberInfoType MemberInfo { get; }
    }
}

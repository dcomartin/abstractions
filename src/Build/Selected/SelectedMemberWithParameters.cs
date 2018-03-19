using System;
using System.Diagnostics;
using System.Reflection;
using Unity.Build.Context;
using Unity.Build.Factory;
using Unity.Build.Pipeline;
using Unity.Utility;

namespace Unity.Build.Selected
{
    /// <summary>
    /// Base class for return of selector policies that need
    /// to keep track of a set of parameter resolvers.
    /// </summary>
    public class SelectedMemberWithParameters : IResolveMethodFactory<Type>
    {
        #region Constructors

        protected SelectedMemberWithParameters(ParameterInfo[] parameters)
        {
            var length = parameters.Length;
            if (0 == length)
            {
                var array = new object[0];
                ResolveMethodFactory = type => (ref ResolutionContext context) => array;
            }
            else
            {
                var factories = new ResolveMethodFactory<Type>[length];

                for (var f = 0; f < length; f++) factories[f] = parameters[f].ToFactory();

                ResolveMethodFactory = type =>
                {
                    var resolvers = new ResolveMethod[length];
                    for (var p = 0; p < length; p++) resolvers[p] = factories[p](type);

                    return (ref ResolutionContext context) =>
                    {
                        var values = new object[length];
                        for (var v = 0; v < length; v++) values[v] = resolvers[v](ref context);
                        return values;
                    };
                };
            }
        }

        protected SelectedMemberWithParameters(ParameterInfo[] parameters, object[] members)
        {
            var length = parameters.Length;
            if (0 == length)
            {
                var array = new object[0];
                ResolveMethodFactory = type => (ref ResolutionContext context) => array;
            }
            else
            {
                var factories = new ResolveMethodFactory<Type>[length];

                if (null == members || 0 == members.Length)
                    for (var f = 0; f < length; f++) factories[f] = parameters[f].ToFactory();
                else
                {
                    Debug.Assert(length == members.Length, "Number of InjectionMembers and paremeters are different.");
                    for (var f = 0; f < length; f++) factories[f] = parameters[f].ToFactory(members[f]);
                }

                ResolveMethodFactory = type =>
                {
                    var resolvers = new ResolveMethod[length];
                    for (var p = 0; p < length; p++) resolvers[p] = factories[p](type);

                    return (ref ResolutionContext context) =>
                    {
                        var values = new object[length];
                        for (var v = 0; v < length; v++) values[v] = resolvers[v](ref context);
                        return values;
                    };
                };
            }
        }



        #endregion


        #region Factory

        public virtual ResolveMethodFactory<Type> ResolveMethodFactory { get; }

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

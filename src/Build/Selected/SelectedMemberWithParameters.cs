using System;
using System.Reflection;
using Unity.Build.Context;
using Unity.Build.Pipeline;
using Unity.Utility;

namespace Unity.Build.Selected
{
    /// <summary>
    /// Base class for return of selector policies that need
    /// to keep track of a set of parameter resolvers.
    /// </summary>
    public class SelectedMemberWithParameters : ICreateResolver<Type>
    {
        #region Constructors

        protected SelectedMemberWithParameters()
        {
        }

        protected SelectedMemberWithParameters(ParameterInfo[] parameters)
        {
            var length = parameters.Length;
            var factories = new CreateResolver<Type>[length];

            for (var f = 0; f < length; f++) factories[f] = parameters[f].ToFactory();

            CreateResolver = type =>
            {
                var resolvers = new Resolve[length];
                for (var p = 0; p < length; p++) resolvers[p] = factories[p](type);

                return (ref ResolutionContext context) =>
                {
                    var values = new object[length];
                    for (var v = 0; v < length; v++) values[v] = resolvers[v](ref context);
                    return values;
                };
            };
        }

        protected SelectedMemberWithParameters(ParameterInfo[] parameters, object[] members)
        {
            var length = parameters.Length;
            var factories = new CreateResolver<Type>[length];

            if (null == members)
                for (var f = 0; f < length; f++) factories[f] = parameters[f].ToFactory();
            else
                for (var f = 0; f < length; f++) factories[f] = parameters[f].ToFactory(members[f]);

            CreateResolver = type =>
            {
                var resolvers = new Resolve[length];
                for (var p = 0; p < length; p++) resolvers[p] = factories[p](type);

                return (ref ResolutionContext context) =>
                {
                    var values = new object[length];
                    for (var v = 0; v < length; v++) values[v] = resolvers[v](ref context);
                    return values;
                };
            };
        }



        #endregion


        #region Factory

        public virtual CreateResolver<Type> CreateResolver { get; }

        #endregion
    }


    public class SelectedMemberWithParameters<TMemberInfoType> : SelectedMemberWithParameters
    {
        protected SelectedMemberWithParameters(TMemberInfoType memberInfo)
        {
            MemberInfo = memberInfo;
        }

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

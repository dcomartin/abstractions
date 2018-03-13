﻿using System.Collections.Generic;
using Unity.Policy;

namespace Unity.Dependency
{
    /// <summary>
    /// Base class for return of selector policies that need
    /// to keep track of a set of parameter resolvers.
    /// </summary>
    public class SelectedMemberWithParameters
    {

        private IList<ResolvePipeline> _resolvers { get; } = new List<ResolvePipeline>();

        /// <summary>
        /// Adds the parameter resolver. Resolvers are assumed
        /// to be in the order of the parameters to the member.
        /// </summary>
        /// <param name="pipeline">The new resolver.</param>
        public void AddParameterResolver(ResolvePipeline pipeline)
        {
            _resolvers.Add(pipeline);
        }

        public IEnumerable<ResolvePipeline> Resolvers => _resolvers;



        #region Legacy

        private readonly List<IResolverPolicy> _parameterResolvers = new List<IResolverPolicy>();

        /// <summary>
        /// Adds the parameter resolver. Resolvers are assumed
        /// to be in the order of the parameters to the member.
        /// </summary>
        /// <param name="newResolver">The new resolver.</param>
        public void AddParameterResolver(IResolverPolicy newResolver)
        {
            _parameterResolvers.Add(newResolver);
        }

        /// <summary>
        /// Gets the parameter resolvers.
        /// </summary>
        /// <returns>An array with the parameter resolvers.</returns>
        public IResolverPolicy[] GetParameterResolvers()
        {
            return _parameterResolvers.ToArray();
        }

        #endregion
    }

    /// <summary>
    /// Base class for return values from selector policies that
    /// return a MemberInfo of some sort plus a list of parameter
    /// keys to look up the parameter resolvers.
    /// </summary>
    public class SelectedMemberWithParameters<TMemberInfoType> : SelectedMemberWithParameters
    {
        /// <summary>
        /// Construct a new <see cref="SelectedMemberWithParameters{TMemberInfoType}"/>, storing
        /// the given member info.
        /// </summary>
        /// <param name="memberInfo">Member info to store.</param>
        protected SelectedMemberWithParameters(TMemberInfoType memberInfo)
        {
            MemberInfo = memberInfo;
        }

        /// <summary>
        /// The member info stored.
        /// </summary>
        protected TMemberInfoType MemberInfo { get; }
    }
}

using System;
using System.Collections.Generic;

namespace Unity.Storage
{
    public interface IPolicySet
    {
        // TODO: Add proper summary
        void Add(Type policyInterface, object policy);

        /// <summary>
        /// Get policy
        /// </summary>
        /// <param name="policyInterface">Type of policy to retrieve</param>
        /// <returns>Instance of the policy or null if none found</returns>
        object Get(Type policyInterface);

        // TODO: Add proper summary
        object Get(Type type, string name, Type policyInterface);

        /// <summary>
        /// Set policy
        /// </summary>
        /// <param name="policyInterface">Type of policy to be set</param>
        /// <param name="policy">Policy instance to be set</param>
        void Set(Type policyInterface, object policy);

        // TODO: Add proper summary
        void Set(Type type, string name, Type policyInterface, object policy);

        /// <summary>
        /// Remove specific policy from the list
        /// </summary>
        /// <param name="policyInterface">Type of policy to be removed</param>
        void Clear(Type policyInterface);

        // TODO: Add proper summary
        void Clear(Type type, string name, Type policyInterface);


        IEnumerable<object> OfType<T>(bool exactMatch = false);
    }



    public static class PolicySetExtensions
    {
        public static T Get<T>(this IPolicySet policySet)
        {
            return (T)policySet.Get(typeof(T));
        }

        public static void Set<T>(this IPolicySet policySet, object policy)
        {
            policySet.Set(typeof(T), policy);
        }

        public static TOut OfType<TType, TOut>(this IPolicySet policySet)
        {
            return (TOut)policySet.Get(typeof(TType));
        }
    }
}

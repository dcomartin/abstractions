using System;
using System.Collections.Generic;
using Unity.Policy;

namespace Unity.Abstractions.Tests.TestData
{
    public class TestPolicySet : IPolicySet
    {
        public IDictionary<Type, object> Dictionary { get; } = new Dictionary<Type, object>();

        public void Clear(Type policyInterface)
        {
            Dictionary.Remove(policyInterface);
        }

        public object Get(Type policyInterface)
        {
            return Dictionary[policyInterface];
        }

        public void Set(Type policyInterface, object policy)
        {
            Dictionary[policyInterface] = policy;
        }
    }
}

using System;
using System.Collections.Generic;
using Unity.Storage;

namespace Unity.Abstractions.Tests.TestData
{
    public class TestPolicySet : IPolicySet
    {
        public IDictionary<Type, object> Dictionary { get; } = new Dictionary<Type, object>();

        public void Clear(Type policyInterface)
        {
            Dictionary.Remove(policyInterface);
        }

        public void Clear(Type type, string name, Type policyInterface)
        {
            throw new NotImplementedException();
        }

        public object Get(Type policyInterface)
        {
            return Dictionary[policyInterface];
        }

        public object Get(Type type, string name, Type policyInterface)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> OfType<T>()
        {
            throw new NotImplementedException();
        }

        public void Set(Type policyInterface, object policy)
        {
            Dictionary[policyInterface] = policy;
        }

        public void Set(Type type, string name, Type policyInterface, object policy)
        {
            throw new NotImplementedException();
        }
    }
}

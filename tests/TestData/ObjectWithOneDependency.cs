using System;

namespace Unity.Abstractions.Tests.TestData
{
    public class ObjectWithOneDependency
    {
        private readonly object _inner;

        public ObjectWithOneDependency(object inner)
        {
            _inner = inner;
        }

        public object InnerObject => _inner;

        public void Validate()
        {
            if (null == _inner) throw new ArgumentNullException(nameof(_inner));
        }
    }
}

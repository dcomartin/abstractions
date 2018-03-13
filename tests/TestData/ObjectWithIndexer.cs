using Unity.Attributes;

namespace Unity.Abstractions.Tests.TestData
{
    public class ObjectWithIndexer
    {
        [Dependency]
        public object this[int index]
        {
            get { return null; }
            set { }
        }

        public bool Validate()
        {
            return true;
        }
    }
}

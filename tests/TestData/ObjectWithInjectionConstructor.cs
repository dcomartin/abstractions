using Unity.Attributes;

namespace Unity.Abstractions.Tests.TestData
{
    public class ObjectWithInjectionConstructor
    {
        public ObjectWithInjectionConstructor(object constructorDependency)
        {
            this.ConstructorDependency = constructorDependency;
        }

        [InjectionConstructor]
        public ObjectWithInjectionConstructor(string s)
        {
            ConstructorDependency = s;
        }

        public object ConstructorDependency { get; }
    }
}

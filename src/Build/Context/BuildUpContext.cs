using System;
using Unity.Builder;
using Unity.Lifetime;

namespace Unity.Pipeline
{
    public ref struct BuildUpContext
    {
        public INamedType Target;

        public IUnityContainer Container;

        public ILifetimeContainer LifetimeContainer;

        public Action<Type> Verify;

        public Func<Type, object> Get;
        public Action<Type, object> Set;

        public Func<Type, string, Type, object> GetPolicy;
        public Action<Type, string, Type, object> SetPolicy;

        public Func<Type, string, object> BuildUp;
    }
}

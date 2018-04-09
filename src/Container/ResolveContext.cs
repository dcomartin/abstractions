using System;
using Unity.Lifetime;
using Unity.Storage;

namespace Unity.Container
{
    public delegate ref ResolveContext ParentDelegate();

    public delegate object ResolveDelegate(Type type, string name);


    public struct ResolveContext
    {
        public ILifetimeContainer LifetimeContainer;

        public IPolicySet Registration;

        public ResolveDelegate Resolve;

        public ParentDelegate Parent;
    }
}

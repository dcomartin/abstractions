using System;
using Unity.Lifetime;
using Unity.Storage;

namespace Unity.Build.Context
{
    public delegate ref ResolutionContext ParentDelegate();

    public delegate object ResolveDelegate(Type type, string name);


    public struct ResolutionContext
    {
        public ILifetimeContainer LifetimeContainer;

        public IPolicySet Registration;

        public ResolveDelegate Resolve;

        public ParentDelegate Parent;
    }
}

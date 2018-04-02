using Unity.Lifetime;
using Unity.Storage;

namespace Unity.Build.Pipeline
{
    public delegate void RegisterPipeline(ILifetimeContainer container, IPolicySet registration, params object[] args);
}

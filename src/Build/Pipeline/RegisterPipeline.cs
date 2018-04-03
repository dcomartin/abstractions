using Unity.Lifetime;
using Unity.Storage;

namespace Unity.Build.Pipeline
{
    public delegate ResolveMethod RegisterPipeline(ILifetimeContainer lifetimeContainer, IPolicySet registration, params object[] args);
}

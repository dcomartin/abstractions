using Unity.Build.Context;

namespace Unity.Build.Pipeline
{
    public delegate object ResolveMethod(ref ResolutionContext context);

    public interface IResolveMethod
    {
        ResolveMethod ResolveMethod { get; }
    }
}

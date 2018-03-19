using Unity.Build.Pipeline;

namespace Unity.Build.Factory
{
    public delegate ResolveMethod ResolveMethodFactory<T>(T type);

    public interface IResolveMethodFactory<T>
    {
        ResolveMethodFactory<T> ResolveMethodFactory { get; }
    }
}

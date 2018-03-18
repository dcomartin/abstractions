using Unity.Build.Context;

namespace Unity.Build.Pipeline
{
    public delegate object Resolve(ref ResolutionContext context);


    public interface IResolve
    {
        Resolve Resolve { get; }
    }

    public delegate Resolve CreateResolver<T>(T type);

    public interface ICreateResolver<T>
    {
        CreateResolver<T> CreateResolver { get; }
    }
}

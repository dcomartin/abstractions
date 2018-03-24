using System.Linq.Expressions;
using Unity.Build.Factory;
using Unity.Build.Pipeline;

namespace Unity.Build.Policy
{
    public interface IResolve<in TData>
    {
        PipelineFactory<TData, ResolveMethod> Resolver { get; }

        PipelineFactory<TData, Expression> Expression { get; }
    }
}

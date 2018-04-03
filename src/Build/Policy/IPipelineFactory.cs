using System.Linq.Expressions;
using Unity.Build.Pipeline;

namespace Unity.Build.Policy
{
    public interface IPipelineFactory<in TData>
    {
        PipelineFactory<TData, ResolveMethod> CreateActivator { get; }

        PipelineFactory<TData, Expression> CreateExpression { get; }
    }
}

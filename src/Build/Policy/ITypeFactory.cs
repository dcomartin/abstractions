using System.Linq.Expressions;
using Unity.Container;

namespace Unity.Build.Policy
{
    public interface ITypeFactory<in TData>
    {
        Factory<TData, ResolvePipeline> CreatePipeline { get; }

        Factory<TData, Expression> CreateExpression { get; }
    }
}

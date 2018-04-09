
using System.Linq.Expressions;

namespace Unity.Container
{
    public delegate object ResolvePipeline(ref ResolveContext context);

    public interface IResolvePipeline
    {
        ResolvePipeline ResolvePipeline { get; }

        Expression ResolveExpression { get; }

    }
}

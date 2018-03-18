
using Unity.Pipeline;

namespace Unity.Build.Pipeline
{
    public delegate object BuildUpPipeline(ref BuildUpContext context);

    public interface IBuildUp
    {
        BuildUpPipeline BuildUpPipeline { get; }
    }
}

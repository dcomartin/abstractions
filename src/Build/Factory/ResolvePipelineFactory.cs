using Unity.Build.Pipeline;
using Unity.Pipeline;

namespace Unity.Build.Factory
{
    public delegate ResolvePipeline ResolvePipelineFactory<T>(T arg);


    public interface IResolvePipelineFactory<T>
    {
        ResolvePipelineFactory<T> ResolvePipeline { get; }
    }
}

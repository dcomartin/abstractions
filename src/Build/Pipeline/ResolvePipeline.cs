
namespace Unity.Build.Pipeline
{
    public delegate object ResolvePipeline(ResolveDependency dependency);

    public interface IResolvePipeline
    {
        ResolvePipeline ResolvePipeline { get; }
    }

}

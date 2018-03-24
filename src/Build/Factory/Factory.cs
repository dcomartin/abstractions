
namespace Unity.Build.Factory
{
    public delegate TPipeline PipelineFactory<in TData, out TPipeline>(TData data);
}

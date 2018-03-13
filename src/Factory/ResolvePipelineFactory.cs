using System;

namespace Unity.Factory
{
    public delegate RegisterPipeline ResolvePipelineFactory(Type type, string name);


    public interface IResolvePipelineFactory
    {
        ResolvePipelineFactory GetResolver { get; }
    }
}

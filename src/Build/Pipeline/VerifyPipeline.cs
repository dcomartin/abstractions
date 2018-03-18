using System;

namespace Unity.Build.Pipeline
{
    public delegate void VerifyPipeline<TType>(TType type);


    public interface IVerifyPipeline<TType>
    {
        VerifyPipeline<TType> VerifyPipeline { get; }
    }

}

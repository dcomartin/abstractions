using System;

namespace Unity.Build.Pipeline
{
    public delegate object ResolveDependency(Type type, string name);

    public interface IResolveDependency
    {
        ResolveDependency ResolveDependency { get; }
    }
}

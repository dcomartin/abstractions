using System;
using Unity.Build.Factory;

namespace Unity.Build.Pipeline
{
    public delegate IResolveMethodFactory<Type> SelectConstructor(Type type);

    public interface ISelectConstructor
    {
        IResolveMethodFactory<Type> SelectConstructor { get; }
    }
}

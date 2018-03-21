using System;
using System.Reflection;
using Unity.Build.Factory;

namespace Unity.Build.Injection
{
    public interface IInjectionMethod : IResolveMethodFactory<Type>
    {
        MethodInfo Method { get; }
    }
}

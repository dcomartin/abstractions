using System;
using System.Reflection;
using Unity.Build.Factory;

namespace Unity.Build.Injection
{
    public interface IInjectionConstructor : IResolveMethodFactory<Type>
    {
        ConstructorInfo Constructor { get; }
    }
}

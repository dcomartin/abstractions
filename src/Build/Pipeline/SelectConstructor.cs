using System;
using Unity.Registration;

namespace Unity.Build.Pipeline
{
    public delegate InjectionConstructor SelectConstructorPipeline(IUnityContainer container, Type type);

}

using System;

namespace Unity.Registration
{
    public delegate object InjectionFactoryDelegate(IUnityContainer container, Type type, string name);
}

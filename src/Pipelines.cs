using System;
using Unity.Dependency;
using Unity.Policy;
using Unity.Resolution;

namespace Unity
{
    public delegate void RegisterPipeline(IUnityContainer container, IPolicySet registration, Type type, string name);


    public delegate SelectedConstructor SelectConstructorPipeline(IUnityContainer container, Type type, string name);




    public delegate IPolicySet GetRegistrationDelegate(Type type, string name);

}

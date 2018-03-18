using System;
using Unity.Policy;

namespace Unity.Build.Pipeline
{
    public delegate void RegisterPipeline(IUnityContainer container, IPolicySet registration, Type type, string name);





    public delegate IPolicySet GetRegistrationDelegate(Type type, string name);

}

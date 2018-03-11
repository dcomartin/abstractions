using System;
using Unity.Builder.Selection;
using Unity.Policy;
using Unity.Resolution;

namespace Unity
{
    public delegate void RegisterPipeline(IUnityContainer container, IPolicySet registration, Type type, string name);


    public delegate SelectedConstructor SelectConstructorPipeline(IUnityContainer container, Type type, string name);


    public delegate object ResolvePipeline(ref ResolutionContext context);


    public delegate IPolicySet GetRegistrationDelegate(Type type, string name);

}

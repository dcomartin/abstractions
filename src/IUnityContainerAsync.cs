using System;
using System.Threading.Tasks;
using Unity.Lifetime;
using Unity.Registration;
using Unity.Resolution;

namespace Unity
{
    public interface IUnityContainerAsync : IUnityContainer
    {
        IUnityContainer RegisterTypeAsync(Type registeredType, string name, Type mappedTo, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers);

        Task<object> ResolveAsync(Type type, string name, params ResolverOverride[] resolverOverrides);
    }
}

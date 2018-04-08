using System;
using Unity.Build.Context;
using Unity.Build.Pipeline;
using Unity.Builder;
using Unity.Storage;

namespace Unity.Registration
{
    /// <summary>
    /// A class that lets you specify a factory method the container
    /// will use to create the object.
    /// </summary>
    /// <remarks>This factory allow using predefined <code>Func&lt;IUnityContainer, Type, string, object&gt;</code> to create types.</remarks>
    public class InjectionFactory : InjectionMember
    {
        #region Constructors

        /// <summary>
        /// Create a new instance of <see cref="InjectionFactory"/> with
        /// the given factory function.
        /// </summary>
        /// <param name="factoryFunc">Factory function.</param>
        public InjectionFactory(Func<IUnityContainer, object> factoryFunc)
        {
            Activator = (Type type) =>
            {
                return (ref ResolutionContext context) => factoryFunc(context.LifetimeContainer.Container);
            };

            Factory = (c, t, s) => factoryFunc(c);
        }

        /// <summary>
        /// Create a new instance of <see cref="InjectionFactory"/> with
        /// the given factory function.
        /// </summary>
        /// <param name="factoryFunc">Factory function.</param>
        public InjectionFactory(Func<IUnityContainer, Type, string, object> factoryFunc)
        {
            Activator = (Type type) =>
            {
                return (ref ResolutionContext context) => factoryFunc(context.LifetimeContainer.Container, type, ((INamedType)context.Registration).Name);
            };

            Factory = factoryFunc ?? throw new ArgumentNullException(nameof(factoryFunc));
        }

        #endregion


        public Func<IUnityContainer, Type, string, object> Factory { get; }


        #region InjectionMember

        public override void AddPolicies(Type registeredType, string name, Type implementationType, IPolicySet policies)
        {
            policies.Set(typeof(InjectionFactory), Factory);
        }

        #endregion
    }
}

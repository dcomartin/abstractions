using System;
using Unity.Builder;
using Unity.Policy;

namespace Unity.Registration
{
    /// <summary>
    /// A class that lets you specify a factory method the container
    /// will use to create the object.
    /// </summary>
    /// <remarks>This factory allow using predefined <code>Func&lt;IUnityContainer, Type, string, object&gt;</code> to create types.</remarks>
    public class InjectionFactory : InjectionMember, IInjectionFactory, IBuildPlanPolicy
    {
        #region Constructors

        /// <summary>
        /// Create a new instance of <see cref="InjectionFactory"/> with
        /// the given factory function.
        /// </summary>
        /// <param name="factoryFunc">Factory function.</param>
        public InjectionFactory(Func<IUnityContainer, object> factoryFunc)
            : this((c, t, s) => factoryFunc(c))
        {
        }

        /// <summary>
        /// Create a new instance of <see cref="InjectionFactory"/> with
        /// the given factory function.
        /// </summary>
        /// <param name="factoryFunc">Factory function.</param>
        public InjectionFactory(Func<IUnityContainer, Type, string, object> factoryFunc)
        {
            Factory = factoryFunc ?? throw new ArgumentNullException(nameof(factoryFunc));
        }

        #endregion


        public Func<IUnityContainer, Type, string, object> Factory { get; }


        #region IInjectionFactory

        public override void AddPolicies(Type registeredType, string name, Type implementationType, IPolicySet policies)
        {
            policies.Set(typeof(IInjectionFactory), Factory);
        }

        /// <summary>
        /// Add policies to the <paramref name="policies"/> to configure the
        /// container to call this constructor with the appropriate parameter values.
        /// </summary>
        /// <param name="serviceType">Type of interface being registered. If no interface,
        /// this will be null. This parameter is ignored in this implementation.</param>
        /// <param name="implementationType">Type of concrete type being registered.</param>
        /// <param name="name">Name used to resolve the type object.</param>
        /// <param name="policies">Policy list to add policies to.</param>
        public override void AddPolicies(Type serviceType, Type implementationType, string name, IPolicyList policies)
        {
            policies.Set(serviceType, name, typeof(IBuildPlanPolicy), this);
        }

        #endregion


        #region IBuildPlanPolicy

        public void BuildUp(IBuilderContext context)
        {
            if ((context ?? throw new ArgumentNullException(nameof(context))).Existing == null)
            {
                context.Existing = Factory(context.Container, context.BuildKey.Type, context.BuildKey.Name);
                context.SetPerBuildSingleton();
            }
        }

        #endregion
    }
}

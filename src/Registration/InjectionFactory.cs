using System;
using Unity.Builder;
using Unity.Container;

namespace Unity.Registration
{
    /// <summary>
    /// A class that lets you specify a factory method the container will use to create the object.
    /// </summary>
    public class InjectionFactory : InjectionMember
    {
        #region Constructors

        /// <summary>
        /// Create a new instance of <see cref="InjectionFactory"/> with the given factory function.
        /// </summary>
        /// <param name="factoryFunc">Factory function.</param>
        public InjectionFactory(Func<IUnityContainer, object> factoryFunc)
        {
            ResolvePipeline = (ref ResolveContext context) => factoryFunc(context.LifetimeContainer.Container);

            CreatePipeline = _ => ResolvePipeline;

            Factory = (c, t, s) => factoryFunc(c);
        }

        /// <summary>
        /// Create a new instance of <see cref="InjectionFactory"/> with the given factory function.
        /// </summary>
        /// <param name="factoryFunc">Factory function.</param>
        public InjectionFactory(Func<IUnityContainer, Type, string, object> factoryFunc)
        {
            ResolvePipeline = (ref ResolveContext context) => 
                factoryFunc(context.LifetimeContainer.Container, ((INamedType)context.Registration).Type, 
                                                                 ((INamedType)context.Registration).Name);
            CreatePipeline = _ => ResolvePipeline;

            Factory = factoryFunc ?? throw new ArgumentNullException(nameof(factoryFunc));
        }

        #endregion


        #region Public Members

        public Func<IUnityContainer, Type, string, object> Factory { get; }
        
        #endregion
    }
}

using System;
using System.Linq.Expressions;
using Unity.Build.Pipeline;
using Unity.Build.Policy;
using Unity.Storage;

namespace Unity.Registration
{
    /// <summary>
    /// Base class for objects that can be used to configure what
    /// class members get injected by the container.
    /// </summary>
    public abstract class InjectionMember : IResolve<Type>
    {
        /// <summary>
        /// Allows injection member to inject necessary policies into registration
        /// </summary>
        /// <param name="registeredType">Registration type</param>
        /// <param name="name">Registration name</param>
        /// <param name="implementationType">Type of the implementation</param>
        /// <param name="set">Set where policies are kept</param>
        public virtual void AddPolicies(Type registeredType, string name, Type implementationType, IPolicySet set)
        {
        }


        #region IResolve

        // Create protectively
        public virtual PipelineFactory<Type, ResolveMethod> Resolver { get; protected set; } = data => throw new InvalidOperationException(Constants.InvalidMember);   
        // TODO: Replace it with abstract

        // Create lazy
        public virtual PipelineFactory<Type, Expression> Expression { get; protected set; }

        #endregion

    }
}

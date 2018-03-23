using System;
using System.Linq.Expressions;
using Unity.Build.Factory;
using Unity.Build.Pipeline;
using Unity.Storage;

namespace Unity.Registration
{
    /// <summary>
    /// Base class for objects that can be used to configure what
    /// class members get injected by the container.
    /// </summary>
    public abstract class InjectionMember
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

        /// <summary>
        /// Method creates a factory to convert open generic recipe into final type resolver.
        /// </summary>
        /// <remarks>
        /// If this member is not NULL it means that either member itself or any of its parameters
        /// are open generic. Before it could be instantiated it has to be made into
        /// Constructed generic. In other words these open generic types required to be
        /// replace with closed types. 
        /// To do so method returned by ResolveFactory needs to be called with target <see cref="Type"/>
        /// </remarks>
        /// <example>
        /// var factory = member.ResolveFactory?.Invoke();
        /// var resolve = factory?.Invoke(typeof(TargetType));
        /// var result  = resolve?.Invoke(...);
        /// </example>
        public virtual ResolveMethodFactory<Type> ResolveFactory { get; protected set; }

        /// <summary>
        /// Method which creates resolver for the InjectionMember. 
        /// </summary>
        /// <remarks>
        /// If this member is not NULL it means that value for this injection member could be
        /// resolved by calling this function. 
        /// </remarks>
        /// <example>
        /// var resolve = member.ResolveMethod?.Invoke();
        /// var result  = resolve?.Invoke(...);
        /// </example>
        public virtual ResolveMethod ResolveMethod { get; protected set; }

        public virtual Func<Expression> CreateExpression { get; protected set; }
    }
}

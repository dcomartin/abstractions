﻿// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Unity.Exceptions;
using Unity.Lifetime;
using Unity.Policy;
using Unity.Resolution;
using Unity.Storage;
using Unity.Strategy;

namespace Unity.Builder
{
    /// <summary>
    /// Represents the context in which a build-up or tear-down operation runs.
    /// </summary>
    public interface IBuilderContext
    {
        /// <summary>
        /// Gets Reference to container.
        /// </summary>
        /// <returns>
        /// Interface for the hosting container
        /// </returns>
        IUnityContainer Container { get; }

        /// <summary>
        /// Gets the head of the strategy chain.
        /// </summary>
        /// <returns>
        /// The strategy that's first in the chain; returns null if there are no
        /// strategies in the chain.
        /// </returns>
        IStrategyChain Strategies { get; }

        /// <summary>
        /// Gets the <see cref="ILifetimeContainer"/> associated with the build.
        /// </summary>
        /// <value>
        /// The <see cref="ILifetimeContainer"/> associated with the build.
        /// </value>
        ILifetimeContainer Lifetime { get; }

        /// <summary>
        /// Gets the original build key for the build operation.
        /// </summary>
        /// <value>
        /// The original build key for the build operation.
        /// </value>
        INamedType OriginalBuildKey { get; }

        /// <summary>
        /// GetOrDefault the current build key for the current build operation.
        /// </summary>
        INamedType BuildKey { get; set; }

        /// <summary>
        /// Set of policies associated with this registration
        /// </summary>
        IPolicySet Registration { get; }

        /// <summary>
        /// Reference to Lifetime manager which requires recovery
        /// </summary>
        IRequiresRecovery RequiresRecovery { get; set; }

        /// <summary>
        /// The set of policies that were passed into this context.
        /// </summary>
        /// <remarks>This returns the policies passed into the context.
        /// Policies added here will remain after buildup completes.</remarks>
        /// <value>The persistent policies for the current context.</value>
        IPolicyList PersistentPolicies { get; }

        /// <summary>
        /// Gets the policies for the current context. 
        /// </summary>
        /// <remarks>Any policies added to this object are transient
        /// and will be erased at the end of the buildup.</remarks>
        /// <value>
        /// The policies for the current context.
        /// </value>
        IPolicyList Policies { get; }

        /// <summary>
        /// The current object being built up or torn down.
        /// </summary>
        /// <value>
        /// The current object being manipulated by the build operation. May
        /// be null if the object hasn't been created yet.</value>
        object Existing { get; set; }

        /// <summary>
        /// Flag indicating if the build operation should continue.
        /// </summary>
        /// <value>true means that building should not call any more
        /// strategies, false means continue to the next strategy.</value>
        bool BuildComplete { get; set; }

        /// <summary>
        /// An object representing what is currently being done in the
        /// build chain. Used to report back errors if there's a failure.
        /// </summary>
        object CurrentOperation { get; set; }

        /// <summary>
        /// The child build context.
        /// </summary>
        IBuilderContext ChildContext { get; }

        /// <summary>
        /// The parent build context.
        /// </summary>
        IBuilderContext ParentContext { get; }
        
        /// <summary>
        /// Add a new set of resolver override objects to the current build operation.
        /// </summary>
        /// <param name="newOverrides"><see cref="ResolverOverride"/> objects to add.</param>
        void AddResolverOverrides(IEnumerable<ResolverOverride> newOverrides);

        /// <summary>
        /// GetOrDefault a <see cref="IResolverPolicy"/> object for the given <paramref name="dependencyType"/>
        /// or null if that @delegate hasn't been overridden.
        /// </summary>
        /// <param name="dependencyType">Type of the @delegate.</param>
        /// <returns>CreateActivator to use, or null if no override matches for the current operation.</returns>
        IResolverPolicy GetOverriddenResolver(Type dependencyType);

        /// <summary>
        /// A method to do a new buildup operation on an existing context.
        /// </summary>
        /// <param name="type">Type of to build</param>
        /// <param name="name">Name of the type to build</param>
        /// <param name="childCustomizationBlock">A delegate that takes a <see cref="IBuilderContext"/>. This
        /// is invoked with the new child context before the build up process starts. This gives callers
        /// the opportunity to customize the context for the build process.</param>
        /// <returns>Resolved object</returns>
        object NewBuildUp(Type type, string name, Action<IBuilderContext> childCustomizationBlock = null);
    }

    /// <summary>
    /// Extension methods to provide convenience overloads over the
    /// <see cref="IBuilderContext"/> interface.
    /// </summary>
    public static class BuilderContextExtensions
    {
        /// <summary>
        /// Add a set of <see cref="ResolverOverride"/>s to the context, specified as a 
        /// variable argument list.
        /// </summary>
        /// <param name="context">Context to add overrides to.</param>
        /// <param name="overrides">The overrides.</param>
        public static void AddResolverOverrides(this IBuilderContext context, params ResolverOverride[] overrides)
        {
            (context ?? throw new ArgumentNullException(nameof(context)))
                .AddResolverOverrides(overrides);
        }


        /// <summary>
        /// A helper method used by the generated IL to set up a PerResolveLifetimeManager lifetime manager
        /// if the current object is such.
        /// </summary>
        /// <param name="context">Current build context.</param>
        public static void SetPerBuildSingleton(this IBuilderContext context)
        {
            var lifetime = (context ?? throw new ArgumentNullException(nameof(context)))
                .Policies.GetOrDefault(typeof(ILifetimePolicy), context.OriginalBuildKey, out _);

            if (lifetime is PerResolveLifetimeManager)
            {
                var perBuildLifetime = new InternalPerResolveLifetimeManager(context.Existing);
                context.Policies.Set(context.OriginalBuildKey.Type, 
                                     context.OriginalBuildKey.Name, 
                                     typeof(ILifetimePolicy), perBuildLifetime);
            }
        }

    }


    // TODO: Unify this with container
    /// <summary>
    /// This is a custom lifetime manager that acts like <see cref="TransientLifetimeManager"/>,
    /// but also provides a signal to the default build plan, marking the type so that
    /// instances are reused across the build up object graph.
    /// </summary>
    internal class InternalPerResolveLifetimeManager : PerResolveLifetimeManager
    {
        /// <summary>
        /// Construct a new <see cref="PerResolveLifetimeManager"/> object that stores the
        /// give value. This value will be returned by <see cref="LifetimeManager.GetValue"/>
        /// but is not stored in the lifetime manager, nor is the value disposed.
        /// This Lifetime manager is intended only for internal use, which is why the
        /// normal <see cref="LifetimeManager.SetValue"/> method is not used here.
        /// </summary>
        /// <param name="value">Value to store.</param>
        public InternalPerResolveLifetimeManager(object value)
        {
            base.value = value;
        }
    }
}

﻿// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using Unity.Builder;
using Unity.Builder.Strategy;
using Unity.Events;
using Unity.Lifetime;
using Unity.Policy;
using Unity.Storage;

namespace Unity.Extension
{
    /// <summary>
    /// The <see cref="ExtensionContext"/> class provides the means for extension objects
    /// to manipulate the internal state of the <see cref="IUnityContainer"/>.
    /// </summary>
    public abstract class ExtensionContext
    {
        #region Container

        /// <summary>
        /// The container that this context is associated with.
        /// </summary>
        /// <value>The <see cref="IUnityContainer"/> object.</value>
        public abstract IUnityContainer Container { get; }

        /// <summary>
        /// The <see cref="ILifetimeContainer"/> that this container uses.
        /// </summary>
        /// <value>The <see cref="ILifetimeContainer"/> is used to manage <see cref="IDisposable"/> objects that the container is managing.</value>
        public abstract ILifetimeContainer Lifetime { get; }

        #endregion


        #region Strategies

        /// <summary>
        /// The strategies this container uses.
        /// </summary>
        /// <value>The <see cref="IStagedFactoryChain{TStrategyType,TStageEnum}"/> that the container uses to build objects.</value>
        public virtual IStagedFactoryChain<BuilderStrategy, UnityBuildStage> Strategies { get; }

        /// <summary>
        /// The strategies this container uses to construct build plans.
        /// </summary>
        /// <value>The <see cref="IStagedFactoryChain{TStrategyType,TStageEnum}"/> that this container uses when creating
        /// build plans.</value>
        public virtual IStagedFactoryChain<BuilderStrategy, BuilderStage> BuildPlanStrategies { get; }

        #endregion


        #region Policy Lists

        /// <summary>
        /// The policies this container uses.
        /// </summary>
        /// <remarks>The <see cref="IPolicyList"/> the that container uses to build objects.</remarks>
        public abstract IPolicyList Policies { get; }

        #endregion


        #region Events

        /// <summary>
        /// This event is raised when the 
        /// <see cref="IUnityContainer.RegisterType(Type,Type,string,LifetimeManager, Unity.Registration.InjectionMember[])"/> 
        /// method, or one of its overloads, is called.
        /// </summary>
        public abstract event EventHandler<RegisterEventArgs> Registering;

        /// <summary>
        /// This event is raised when the <see cref="IUnityContainer.RegisterInstance(Type,string,object,LifetimeManager)"/> method,
        /// or one of its overloads, is called.
        /// </summary>
        public abstract event EventHandler<RegisterInstanceEventArgs> RegisteringInstance;

        /// <summary>
        /// This event is raised when the <see cref="IUnityContainer.CreateChildContainer"/> method is called, providing 
        /// the newly created child container to extensions to act on as they see fit.
        /// </summary>
        public abstract event EventHandler<ChildContainerCreatedEventArgs> ChildContainerCreated;

        #endregion
    }
}

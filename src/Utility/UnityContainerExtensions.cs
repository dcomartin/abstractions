// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Extension;
using Unity.Lifetime;
using Unity.Registration;
using Unity.Resolution;

namespace Unity
{
    /// <summary>
    /// Extension class that adds a set of convenience overloads to the
    /// <see cref="IUnityContainer"/> interface.
    /// </summary>
    public static class UnityContainerExtensions
    {
        #region RegisterType overloads

        
        #region Generics overloads

        /// <summary>
        /// Register a type with specific members to be injected.
        /// </summary>
        /// <typeparam name="TRegistered">Type this registration is for.</typeparam>
        /// <param name="container">Container to configure.</param>
        /// <param name="injectionMembers">Injection configuration objects.</param>
        /// <returns>The <see cref="Unity.IUnityContainer"/> object that this method was called on (this in C#, Me in Visual Basic).</returns>
        public static IUnityContainer RegisterType<TRegistered>(this IUnityContainer container, params InjectionMember[] injectionMembers)
        {
            return (container ?? throw new ArgumentNullException(nameof(container))).RegisterType(typeof(TRegistered), null, null, null, injectionMembers);
        }

        /// <summary>
        /// Register a <see cref="LifetimeManager"/> for the given type with the container.
        /// No type mapping is performed for this type.
        /// </summary>
        /// <typeparam name="TRegistered">The type to apply the <paramref name="lifetimeManager"/> to.</typeparam>
        /// <param name="container">Container to configure.</param>
        /// <param name="lifetimeManager">The <see cref="LifetimeManager"/> that controls the lifetime
        /// of the returned instance.</param>
        /// <param name="injectionMembers">Injection configuration objects.</param>
        /// <returns>The <see cref="Unity.IUnityContainer"/> object that this method was called on (this in C#, Me in Visual Basic).</returns>
        public static IUnityContainer RegisterType<TRegistered>(this IUnityContainer container, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
        {
            return (container ?? throw new ArgumentNullException(nameof(container))).RegisterType(typeof(TRegistered), null, null, lifetimeManager, injectionMembers);
        }

        /// <summary>
        /// Register a <see cref="LifetimeManager"/> for the given type with the container.
        /// No type mapping is performed for this type.
        /// </summary>
        /// <typeparam name="TRegistered">The type to configure injection on.</typeparam>
        /// <param name="container">Container to configure.</param>
        /// <param name="name">Name that will be used to request the type.</param>
        /// <param name="injectionMembers">Injection configuration objects.</param>
        /// <returns>The <see cref="Unity.IUnityContainer"/> object that this method was called on (this in C#, Me in Visual Basic).</returns>
        public static IUnityContainer RegisterType<TRegistered>(this IUnityContainer container, string name, params InjectionMember[] injectionMembers)
        {
            return (container ?? throw new ArgumentNullException(nameof(container))).RegisterType(typeof(TRegistered), name, null, null, injectionMembers);
        }

        /// <summary>
        /// Register a <see cref="LifetimeManager"/> for the given type and name with the container.
        /// No type mapping is performed for this type.
        /// </summary>
        /// <typeparam name="TRegistered">The type to apply the <paramref name="lifetimeManager"/> to.</typeparam>
        /// <param name="container">Container to configure.</param>
        /// <param name="name">Name that will be used to request the type.</param>
        /// <param name="lifetimeManager">The <see cref="LifetimeManager"/> that controls the lifetime
        /// of the returned instance.</param>
        /// <param name="injectionMembers">Injection configuration objects.</param>
        /// <returns>The <see cref="Unity.IUnityContainer"/> object that this method was called on (this in C#, Me in Visual Basic).</returns>
        public static IUnityContainer RegisterType<TRegistered>(this IUnityContainer container, string name, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
        {
            return (container ?? throw new ArgumentNullException(nameof(container))).RegisterType(typeof(TRegistered), name, null, lifetimeManager, injectionMembers);
        }

        /// <summary>
        /// Register a type mapping with the container.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is used to tell the container that when asked for type <typeparamref name="TRegistered"/>,
        /// actually return an instance of type <typeparamref name="TMappedTo"/>. This is very useful for
        /// getting instances of interfaces.
        /// </para>
        /// <para>
        /// This overload registers a default mapping and transient lifetime.
        /// </para>
        /// </remarks>
        /// <typeparam name="TRegistered"><see cref="Type"/> that will be requested.</typeparam>
        /// <typeparam name="TMappedTo"><see cref="Type"/> that will actually be returned.</typeparam>
        /// <param name="container">Container to configure.</param>
        /// <param name="injectionMembers">Injection configuration objects.</param>
        /// <returns>The <see cref="Unity.IUnityContainer"/> object that this method was called on (this in C#, Me in Visual Basic).</returns>
        public static IUnityContainer RegisterType<TRegistered, TMappedTo>(this IUnityContainer container, params InjectionMember[] injectionMembers) where TMappedTo : TRegistered
        {
            return (container ?? throw new ArgumentNullException(nameof(container))).RegisterType(typeof(TRegistered), null, typeof(TMappedTo), null, injectionMembers);
        }

        /// <summary>
        /// Register a type mapping with the container, where the created instances will use
        /// the given <see cref="LifetimeManager"/>.
        /// </summary>
        /// <typeparam name="TRegistered"><see cref="Type"/> that will be requested.</typeparam>
        /// <typeparam name="TMappedTo"><see cref="Type"/> that will actually be returned.</typeparam>
        /// <param name="container">Container to configure.</param>
        /// <param name="lifetimeManager">The <see cref="LifetimeManager"/> that controls the lifetime
        /// of the returned instance.</param>
        /// <param name="injectionMembers">Injection configuration objects.</param>
        /// <returns>The <see cref="Unity.IUnityContainer"/> object that this method was called on (this in C#, Me in Visual Basic).</returns>
        public static IUnityContainer RegisterType<TRegistered, TMappedTo>(this IUnityContainer container, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers) where TMappedTo : TRegistered
        {
            return (container ?? throw new ArgumentNullException(nameof(container))).RegisterType(typeof(TRegistered), null, typeof(TMappedTo), lifetimeManager, injectionMembers);
        }

        /// <summary>
        /// Register a type mapping with the container.
        /// </summary>
        /// <remarks>
        /// This method is used to tell the container that when asked for type <typeparamref name="TRegistered"/>,
        /// actually return an instance of type <typeparamref name="TMappedTo"/>. This is very useful for
        /// getting instances of interfaces.
        /// </remarks>
        /// <typeparam name="TRegistered"><see cref="Type"/> that will be requested.</typeparam>
        /// <typeparam name="TMappedTo"><see cref="Type"/> that will actually be returned.</typeparam>
        /// <param name="container">Container to configure.</param>
        /// <param name="name">Name of this mapping.</param>
        /// <param name="injectionMembers">Injection configuration objects.</param>
        /// <returns>The <see cref="Unity.IUnityContainer"/> object that this method was called on (this in C#, Me in Visual Basic).</returns>
        public static IUnityContainer RegisterType<TRegistered, TMappedTo>(this IUnityContainer container, string name, params InjectionMember[] injectionMembers)
            where TMappedTo : TRegistered
        {
            return (container ?? throw new ArgumentNullException(nameof(container))).RegisterType(typeof(TRegistered), name, typeof(TMappedTo), null, injectionMembers);
        }

        /// <summary>
        /// Register a type mapping with the container, where the created instances will use
        /// the given <see cref="LifetimeManager"/>.
        /// </summary>
        /// <typeparam name="TRegistered"><see cref="Type"/> that will be requested.</typeparam>
        /// <typeparam name="TMappedTo"><see cref="Type"/> that will actually be returned.</typeparam>
        /// <param name="container">Container to configure.</param>
        /// <param name="name">Name to use for registration, null if a default registration.</param>
        /// <param name="lifetimeManager">The <see cref="LifetimeManager"/> that controls the lifetime
        /// of the returned instance.</param>
        /// <param name="injectionMembers">Injection configuration objects.</param>
        /// <returns>The <see cref="Unity.IUnityContainer"/> object that this method was called on (this in C#, Me in Visual Basic).</returns>
        public static IUnityContainer RegisterType<TRegistered, TMappedTo>(this IUnityContainer container, string name, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers) where TMappedTo : TRegistered
        {
            return (container ?? throw new ArgumentNullException(nameof(container))).RegisterType(typeof(TRegistered), name, typeof(TMappedTo), lifetimeManager, injectionMembers);
        }


        /// <summary>
        /// Register a type with specific members to be injected as singleton.
        /// </summary>
        /// <typeparam name="TRegistered">Type this registration is for.</typeparam>
        /// <param name="container">Container to configure.</param>
        /// <param name="injectionMembers">Injection configuration objects.</param>
        /// <returns>The <see cref="Unity.IUnityContainer"/> object that this method was called on (this in C#, Me in Visual Basic).</returns>
        public static IUnityContainer RegisterSingleton<TRegistered>(this IUnityContainer container, params InjectionMember[] injectionMembers)
        {
            return (container ?? throw new ArgumentNullException(nameof(container))).RegisterType(typeof(TRegistered), null, null, new ContainerControlledLifetimeManager(), injectionMembers);
        }

        /// <summary>
        /// Register type as a singleton.
        /// </summary>
        /// <typeparam name="TRegistered">The type to configure injection on.</typeparam>
        /// <param name="container">Container to configure.</param>
        /// <param name="name">Name that will be used to request the type.</param>
        /// <param name="injectionMembers">Injection configuration objects.</param>
        /// <returns>The <see cref="Unity.IUnityContainer"/> object that this method was called on (this in C#, Me in Visual Basic).</returns>
        public static IUnityContainer RegisterSingleton<TRegistered>(this IUnityContainer container, string name, params InjectionMember[] injectionMembers)
        {
            return (container ?? throw new ArgumentNullException(nameof(container))).RegisterType(typeof(TRegistered), name, null, new ContainerControlledLifetimeManager(), injectionMembers);
        }

        /// <summary>
        /// Register a type mapping with the container.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is used to tell the container that when asked for type <typeparamref name="TRegistered"/>,
        /// actually return an instance of type <typeparamref name="TMappedTo"/>. This is very useful for
        /// getting instances of interfaces.
        /// </para>
        /// <para>
        /// This overload registers a default mapping and transient lifetime.
        /// </para>
        /// </remarks>
        /// <typeparam name="TRegistered"><see cref="Type"/> that will be requested.</typeparam>
        /// <typeparam name="TMappedTo"><see cref="Type"/> that will actually be returned.</typeparam>
        /// <param name="container">Container to configure.</param>
        /// <param name="injectionMembers">Injection configuration objects.</param>
        /// <returns>The <see cref="Unity.IUnityContainer"/> object that this method was called on (this in C#, Me in Visual Basic).</returns>
        public static IUnityContainer RegisterSingleton<TRegistered, TMappedTo>(this IUnityContainer container, params InjectionMember[] injectionMembers) where TMappedTo : TRegistered
        {
            return (container ?? throw new ArgumentNullException(nameof(container))).RegisterType(typeof(TRegistered), null, typeof(TMappedTo), new ContainerControlledLifetimeManager(), injectionMembers);
        }

        /// <summary>
        /// Register a type mapping as singleton.
        /// </summary>
        /// <remarks>
        /// This method is used to tell the container that when asked for type <typeparamref name="TRegistered"/>,
        /// actually return an instance of type <typeparamref name="TMappedTo"/>. This is very useful for
        /// getting instances of interfaces.
        /// </remarks>
        /// <typeparam name="TRegistered"><see cref="Type"/> that will be requested.</typeparam>
        /// <typeparam name="TMappedTo"><see cref="Type"/> that will actually be returned.</typeparam>
        /// <param name="container">Container to configure.</param>
        /// <param name="name">Name of this mapping.</param>
        /// <param name="injectionMembers">Injection configuration objects.</param>
        /// <returns>The <see cref="Unity.IUnityContainer"/> object that this method was called on (this in C#, Me in Visual Basic).</returns>
        public static IUnityContainer RegisterSingleton<TRegistered, TMappedTo>(this IUnityContainer container, string name, params InjectionMember[] injectionMembers)
            where TMappedTo : TRegistered
        {
            return (container ?? throw new ArgumentNullException(nameof(container))).RegisterType(typeof(TRegistered), name, typeof(TMappedTo), new ContainerControlledLifetimeManager(), injectionMembers);
        }

        #endregion

        
        #region Non-generics overloads

        /// <summary>
        /// Register a type with specific members to be injected.
        /// </summary>
        /// <param name="container">Container to configure.</param>
        /// <param name="registeredType">Type this registration is for.</param>
        /// <param name="injectionMembers">Injection configuration objects.</param>
        /// <returns>The <see cref="Unity.IUnityContainer"/> object that this method was called on (this in C#, Me in Visual Basic).</returns>
        public static IUnityContainer RegisterType(this IUnityContainer container, Type registeredType, params InjectionMember[] injectionMembers)
        {
            return (container ?? throw new ArgumentNullException(nameof(container))).RegisterType(registeredType, null, null, null, injectionMembers);
        }

        /// <summary>
        /// Register a <see cref="LifetimeManager"/> for the given type and name with the container.
        /// No type mapping is performed for this type.
        /// </summary>
        /// <param name="container">Container to configure.</param>
        /// <param name="registeredType">The <see cref="Type"/> to apply the <paramref name="lifetimeManager"/> to.</param>
        /// <param name="lifetimeManager">The <see cref="LifetimeManager"/> that controls the lifetime
        /// of the returned instance.</param>
        /// <param name="injectionMembers">Injection configuration objects.</param>
        /// <returns>The <see cref="Unity.IUnityContainer"/> object that this method was called on (this in C#, Me in Visual Basic).</returns>
        public static IUnityContainer RegisterType(this IUnityContainer container, Type registeredType, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
        {
            return (container ?? throw new ArgumentNullException(nameof(container))).RegisterType(registeredType, null, null, lifetimeManager, injectionMembers);
        }

        /// <summary>
        /// Register a <see cref="LifetimeManager"/> for the given type and name with the container.
        /// No type mapping is performed for this type.
        /// </summary>
        /// <param name="container">Container to configure.</param>
        /// <param name="registeredType">The <see cref="Type"/> to configure in the container.</param>
        /// <param name="name">Name to use for registration, null if a default registration.</param>
        /// <param name="injectionMembers">Injection configuration objects.</param>
        /// <returns>The <see cref="Unity.IUnityContainer"/> object that this method was called on (this in C#, Me in Visual Basic).</returns>
        public static IUnityContainer RegisterType(this IUnityContainer container, Type registeredType, string name, params InjectionMember[] injectionMembers)
        {
            return (container ?? throw new ArgumentNullException(nameof(container))).RegisterType(registeredType, name, null, null, injectionMembers);
        }

        /// <summary>
        /// Register a <see cref="LifetimeManager"/> for the given type and name with the container.
        /// No type mapping is performed for this type.
        /// </summary>
        /// <param name="container">Container to configure.</param>
        /// <param name="registeredType">The <see cref="Type"/> to apply the <paramref name="lifetimeManager"/> to.</param>
        /// <param name="name">Name to use for registration, null if a default registration.</param>
        /// <param name="lifetimeManager">The <see cref="LifetimeManager"/> that controls the lifetime
        /// of the returned instance.</param>
        /// <param name="injectionMembers">Injection configuration objects.</param>
        /// <returns>The <see cref="Unity.IUnityContainer"/> object that this method was called on (this in C#, Me in Visual Basic).</returns>
        public static IUnityContainer RegisterType(this IUnityContainer container, Type registeredType, string name, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
        {
            return (container ?? throw new ArgumentNullException(nameof(container))).RegisterType(registeredType, name, null, lifetimeManager, injectionMembers);
        }

        /// <summary>
        /// Register a type mapping with the container.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is used to tell the container that when asked for type <paramref name="registeredType"/>,
        /// actually return an instance of type <paramref name="mappedTo"/>. This is very useful for
        /// getting instances of interfaces.
        /// </para>
        /// <para>
        /// This overload registers a default mapping.
        /// </para>
        /// </remarks>
        /// <param name="container">Container to configure.</param>
        /// <param name="registeredType"><see cref="Type"/> that will be requested.</param>
        /// <param name="mappedTo"><see cref="Type"/> that will actually be returned.</param>
        /// <param name="injectionMembers">Injection configuration objects.</param>
        /// <returns>The <see cref="Unity.IUnityContainer"/> object that this method was called on (this in C#, Me in Visual Basic).</returns>
        public static IUnityContainer RegisterType(this IUnityContainer container, Type registeredType, Type mappedTo, params InjectionMember[] injectionMembers)
        {
            return (container ?? throw new ArgumentNullException(nameof(container))).RegisterType(registeredType, null, mappedTo, null, injectionMembers);
        }

        /// <summary>
        /// Register a type mapping with the container.
        /// </summary>
        /// <remarks>
        /// This method is used to tell the container that when asked for type <paramref name="registeredType"/>,
        /// actually return an instance of type <paramref name="mappedTo"/>. This is very useful for
        /// getting instances of interfaces.
        /// </remarks>
        /// <param name="container">Container to configure.</param>
        /// <param name="registeredType"><see cref="Type"/> that will be requested.</param>
        /// <param name="mappedTo"><see cref="Type"/> that will actually be returned.</param>
        /// <param name="name">Name to use for registration, null if a default registration.</param>
        /// <param name="injectionMembers">Injection configuration objects.</param>
        /// <returns>The <see cref="Unity.IUnityContainer"/> object that this method was called on (this in C#, Me in Visual Basic).</returns>
        public static IUnityContainer RegisterType(this IUnityContainer container, Type registeredType, Type mappedTo, string name, params InjectionMember[] injectionMembers)
        {
            return (container ?? throw new ArgumentNullException(nameof(container))).RegisterType(registeredType, name, mappedTo, null, injectionMembers);
        }

        /// <summary>
        /// Register a type mapping with the container, where the created instances will use
        /// the given <see cref="LifetimeManager"/>.
        /// </summary>
        /// <param name="container">Container to configure.</param>
        /// <param name="registeredType"><see cref="Type"/> that will be requested.</param>
        /// <param name="mappedTo"><see cref="Type"/> that will actually be returned.</param>
        /// <param name="lifetimeManager">The <see cref="LifetimeManager"/> that controls the lifetime
        /// of the returned instance.</param>
        /// <param name="injectionMembers">Injection configuration objects.</param>
        /// <returns>The <see cref="Unity.IUnityContainer"/> object that this method was called on (this in C#, Me in Visual Basic).</returns>
        public static IUnityContainer RegisterType(this IUnityContainer container, Type registeredType, Type mappedTo, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
        {
            return (container ?? throw new ArgumentNullException(nameof(container))).RegisterType(registeredType, null, mappedTo, lifetimeManager, injectionMembers);
        }


        /// <summary>
        /// Register a type with specific members to be injected.
        /// </summary>
        /// <param name="container">Container to configure.</param>
        /// <param name="registeredType">Type this registration is for.</param>
        /// <param name="injectionMembers">Injection configuration objects.</param>
        /// <returns>The <see cref="Unity.IUnityContainer"/> object that this method was called on (this in C#, Me in Visual Basic).</returns>
        public static IUnityContainer RegisterSingleton(this IUnityContainer container, Type registeredType, params InjectionMember[] injectionMembers)
        {
            return (container ?? throw new ArgumentNullException(nameof(container))).RegisterType(registeredType, null, null, new ContainerControlledLifetimeManager(), injectionMembers);
        }

        /// <summary>
        /// Register a <see cref="LifetimeManager"/> for the given type and name with the container.
        /// No type mapping is performed for this type.
        /// </summary>
        /// <param name="container">Container to configure.</param>
        /// <param name="registeredType">The <see cref="Type"/> to configure in the container.</param>
        /// <param name="name">Name to use for registration, null if a default registration.</param>
        /// <param name="injectionMembers">Injection configuration objects.</param>
        /// <returns>The <see cref="Unity.IUnityContainer"/> object that this method was called on (this in C#, Me in Visual Basic).</returns>
        public static IUnityContainer RegisterSingleton(this IUnityContainer container, Type registeredType, string name, params InjectionMember[] injectionMembers)
        {
            return (container ?? throw new ArgumentNullException(nameof(container))).RegisterType(registeredType, name, null, new ContainerControlledLifetimeManager(), injectionMembers);
        }

        /// <summary>
        /// Register a type mapping with the container.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is used to tell the container that when asked for type <paramref name="registeredType"/>,
        /// actually return an instance of type <paramref name="mappedTo"/>. This is very useful for
        /// getting instances of interfaces.
        /// </para>
        /// <para>
        /// This overload registers a default mapping.
        /// </para>
        /// </remarks>
        /// <param name="container">Container to configure.</param>
        /// <param name="registeredType"><see cref="Type"/> that will be requested.</param>
        /// <param name="mappedTo"><see cref="Type"/> that will actually be returned.</param>
        /// <param name="injectionMembers">Injection configuration objects.</param>
        /// <returns>The <see cref="Unity.IUnityContainer"/> object that this method was called on (this in C#, Me in Visual Basic).</returns>
        public static IUnityContainer RegisterSingleton(this IUnityContainer container, Type registeredType, Type mappedTo, params InjectionMember[] injectionMembers)
        {
            return (container ?? throw new ArgumentNullException(nameof(container))).RegisterType(registeredType, null, mappedTo, new ContainerControlledLifetimeManager(), injectionMembers);
        }

        /// <summary>
        /// Register a type mapping with the container.
        /// </summary>
        /// <remarks>
        /// This method is used to tell the container that when asked for type <paramref name="registeredType"/>,
        /// actually return an instance of type <paramref name="mappedTo"/>. This is very useful for
        /// getting instances of interfaces.
        /// </remarks>
        /// <param name="container">Container to configure.</param>
        /// <param name="registeredType"><see cref="Type"/> that will be requested.</param>
        /// <param name="mappedTo"><see cref="Type"/> that will actually be returned.</param>
        /// <param name="name">Name to use for registration, null if a default registration.</param>
        /// <param name="injectionMembers">Injection configuration objects.</param>
        /// <returns>The <see cref="Unity.IUnityContainer"/> object that this method was called on (this in C#, Me in Visual Basic).</returns>
        public static IUnityContainer RegisterSingleton(this IUnityContainer container, Type registeredType, Type mappedTo, string name, params InjectionMember[] injectionMembers)
        {
            return (container ?? throw new ArgumentNullException(nameof(container))).RegisterType(registeredType, name, mappedTo, new ContainerControlledLifetimeManager(), injectionMembers);
        }

        #endregion

        
        #endregion

        
        #region RegisterInstance overloads

        
        #region Generics overloads

        /// <summary>
        /// Register an instance with the container.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Instance registration is much like setting a type as a singleton, except that instead
        /// of the container creating the instance the first time it is requested, the user
        /// creates the instance ahead of type and adds that instance to the container.
        /// </para>
        /// <para>
        /// This overload does a default registration and has the container take over the lifetime of the instance.</para>
        /// </remarks>
        /// <typeparam name="TInterface">Type of instance to register (may be an implemented interface instead of the full type).</typeparam>
        /// <param name="container">Container to configure.</param>
        /// <param name="instance">Object to returned.</param>
        /// <returns>The <see cref="Unity.IUnityContainer"/> object that this method was called on (this in C#, Me in Visual Basic).</returns>
        public static IUnityContainer RegisterInstance<TInterface>(this IUnityContainer container, TInterface instance)
        {
            return (container ?? throw new ArgumentNullException(nameof(container)))
                .RegisterInstance(typeof(TInterface), null, instance, new ContainerControlledLifetimeManager());
        }

        /// <summary>
        /// Register an instance with the container.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Instance registration is much like setting a type as a singleton, except that instead
        /// of the container creating the instance the first time it is requested, the user
        /// creates the instance ahead of type and adds that instance to the container.
        /// </para>
        /// <para>
        /// This overload does a default registration (name = null).
        /// </para>
        /// </remarks>
        /// <typeparam name="TRegisteredType">Type of instance to register (may be an implemented interface instead of the full type).</typeparam>
        /// <param name="container">Container to configure.</param>
        /// <param name="instance">Object to returned.</param>
        /// <param name="lifetimeManager">
        /// <see cref="LifetimeManager"/> object that controls how this instance will be managed by the container.</param>
        /// <returns>The <see cref="Unity.IUnityContainer"/> object that this method was called on (this in C#, Me in Visual Basic).</returns>
        public static IUnityContainer RegisterInstance<TRegisteredType>(this IUnityContainer container, TRegisteredType instance, LifetimeManager lifetimeManager)
        {
            return (container ?? throw new ArgumentNullException(nameof(container)))
                .RegisterInstance(typeof(TRegisteredType), null, instance, lifetimeManager);
        }

        /// <summary>
        /// Register an instance with the container.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Instance registration is much like setting a type as a singleton, except that instead
        /// of the container creating the instance the first time it is requested, the user
        /// creates the instance ahead of type and adds that instance to the container.
        /// </para>
        /// <para>
        /// This overload automatically has the container take ownership of the <paramref name="instance"/>.</para>
        /// </remarks>
        /// <typeparam name="TRegisteredType">Type of instance to register (may be an implemented interface instead of the full type).</typeparam>
        /// <param name="instance">Object to returned.</param>
        /// <param name="container">Container to configure.</param>
        /// <param name="name">Name for registration.</param>
        /// <returns>The <see cref="Unity.IUnityContainer"/> object that this method was called on (this in C#, Me in Visual Basic).</returns>
        public static IUnityContainer RegisterInstance<TRegisteredType>(this IUnityContainer container, string name, TRegisteredType instance)
        {
            return (container ?? throw new ArgumentNullException(nameof(container)))
                .RegisterInstance(typeof(TRegisteredType), name, instance, new ContainerControlledLifetimeManager());
        }

        /// <summary>
        /// Register an instance with the container.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Instance registration is much like setting a type as a singleton, except that instead
        /// of the container creating the instance the first time it is requested, the user
        /// creates the instance ahead of type and adds that instance to the container.
        /// </para>
        /// </remarks>
        /// <typeparam name="TRegisteredType">Type of instance to register (may be an implemented interface instead of the full type).</typeparam>
        /// <param name="instance">Object to returned.</param>
        /// <param name="container">Container to configure.</param>
        /// <param name="name">Name for registration.</param>
        /// <param name="lifetimeManager">
        /// <see cref="LifetimeManager"/> object that controls how this instance will be managed by the container.</param>
        /// <returns>The <see cref="Unity.IUnityContainer"/> object that this method was called on (this in C#, Me in Visual Basic).</returns>
        public static IUnityContainer RegisterInstance<TRegisteredType>(this IUnityContainer container, string name, TRegisteredType instance, LifetimeManager lifetimeManager)
        {
            return (container ?? throw new ArgumentNullException(nameof(container))).RegisterInstance(typeof(TRegisteredType), name, instance, lifetimeManager);
        }

        #endregion


        #region Non-generic overloads

        /// <summary>
        /// Register an instance with the container.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Instance registration is much like setting a type as a singleton, except that instead
        /// of the container creating the instance the first time it is requested, the user
        /// creates the instance ahead of type and adds that instance to the container.
        /// </para>
        /// <para>
        /// This overload does a default registration and has the container take over the lifetime of the instance.</para>
        /// </remarks>
        /// <param name="container">Container to configure.</param>
        /// <param name="registeredType">Type of instance to register (may be an implemented interface instead of the full type).</param>
        /// <param name="instance">Object to returned.</param>
        /// <returns>The <see cref="Unity.IUnityContainer"/> object that this method was called on (this in C#, Me in Visual Basic).</returns>
        public static IUnityContainer RegisterInstance(this IUnityContainer container, Type registeredType, object instance)
        {
            return (container ?? throw new ArgumentNullException(nameof(container)))
                .RegisterInstance(registeredType, null, instance, new ContainerControlledLifetimeManager());
        }

        /// <summary>
        /// Register an instance with the container.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Instance registration is much like setting a type as a singleton, except that instead
        /// of the container creating the instance the first time it is requested, the user
        /// creates the instance ahead of type and adds that instance to the container.
        /// </para>
        /// <para>
        /// This overload does a default registration (name = null).
        /// </para>
        /// </remarks>
        /// <param name="container">Container to configure.</param>
        /// <param name="registeredType">Type of instance to register (may be an implemented interface instead of the full type).</param>
        /// <param name="instance">Object to returned.</param>
        /// <param name="lifetimeManager">
        /// <see cref="LifetimeManager"/> object that controls how this instance will be managed by the container.</param>
        /// <returns>The <see cref="Unity.IUnityContainer"/> object that this method was called on (this in C#, Me in Visual Basic).</returns>
        public static IUnityContainer RegisterInstance(this IUnityContainer container, Type registeredType, object instance, LifetimeManager lifetimeManager)
        {
            return (container ?? throw new ArgumentNullException(nameof(container)))
                .RegisterInstance(registeredType, null, instance, lifetimeManager);
        }

        /// <summary>
        /// Register an instance with the container.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Instance registration is much like setting a type as a singleton, except that instead
        /// of the container creating the instance the first time it is requested, the user
        /// creates the instance ahead of type and adds that instance to the container.
        /// </para>
        /// <para>
        /// This overload automatically has the container take ownership of the <paramref name="instance"/>.</para>
        /// </remarks>
        /// <param name="container">Container to configure.</param>
        /// <param name="registeredType">Type of instance to register (may be an implemented interface instead of the full type).</param>
        /// <param name="instance">Object to returned.</param>
        /// <param name="name">Name for registration.</param>
        /// <returns>The <see cref="Unity.IUnityContainer"/> object that this method was called on (this in C#, Me in Visual Basic).</returns>
        public static IUnityContainer RegisterInstance(this IUnityContainer container, Type registeredType, string name, object instance)
        {
            return (container ?? throw new ArgumentNullException(nameof(container)))
                .RegisterInstance(registeredType, name, instance, new ContainerControlledLifetimeManager());
        }

        #endregion

        
        #endregion


        #region ResolveMethod overloads

        /// <summary>
        /// ResolveMethod an instance of the default requested type from the container.
        /// </summary>
        /// <typeparam name="TResolvedType"><see cref="Type"/> of object to get from the container.</typeparam>
        /// <param name="container">Container to resolve from.</param>
        /// <param name="overrides">Any overrides for the resolve call.</param>
        /// <returns>The retrieved object.</returns>
        public static TResolvedType Resolve<TResolvedType>(this IUnityContainer container, params ResolverOverride[] overrides)
        {
            return (TResolvedType)(container ?? throw new ArgumentNullException(nameof(container))).Resolve(typeof(TResolvedType), null, overrides);
        }

        /// <summary>
        /// ResolveMethod an instance of the requested type with the given name from the container.
        /// </summary>
        /// <typeparam name="TResolvedType"><see cref="Type"/> of object to get from the container.</typeparam>
        /// <param name="container">Container to resolve from.</param>
        /// <param name="name">Name of the object to retrieve.</param>
        /// <param name="overrides">Any overrides for the resolve call.</param>
        /// <returns>The retrieved object.</returns>
        public static TResolvedType Resolve<TResolvedType>(this IUnityContainer container, string name, params ResolverOverride[] overrides)
        {
            return (TResolvedType)(container ?? throw new ArgumentNullException(nameof(container))).Resolve(typeof(TResolvedType), name, overrides);
        }

        /// <summary>
        /// ResolveMethod an instance of the default requested type from the container.
        /// </summary>
        /// <param name="container">Container to resolve from.</param>
        /// <param name="type"><see cref="Type"/> of object to get from the container.</param>
        /// <param name="overrides">Any overrides for the resolve call.</param>
        /// <returns>The retrieved object.</returns>
        public static object Resolve(this IUnityContainer container, Type type, params ResolverOverride[] overrides)
        {
            return (container ?? throw new ArgumentNullException(nameof(container))).Resolve(type, null, overrides);
        }

        #endregion


        #region ResolveAll overloads

        /// <summary>
        /// Return instances of all registered types requested.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is useful if you've registered multiple types with the same
        /// <see cref="Type"/> but different names.
        /// </para>
        /// <para>
        /// Be aware that this method does NOT return an instance for the default (unnamed) registration.
        /// </para>
        /// </remarks>
        /// <param name="container">Container to resolve from.</param>
        /// <param name="type">The type requested.</param>
        /// <param name="resolverOverrides">Any overrides for the resolve calls.</param>
        /// <returns>Set of objects of type <paramref name="type"/>.</returns>
        public static IEnumerable<object> ResolveAll(this IUnityContainer container, Type type, params ResolverOverride[] resolverOverrides)
        {
            var result = (container ?? throw new ArgumentNullException(nameof(container))).Resolve((type ?? throw new ArgumentNullException(nameof(type))).MakeArrayType(), resolverOverrides);
            return result is IEnumerable<object> objects ? objects : ((Array)result).Cast<object>();
        }

        /// <summary>
        /// Return instances of all registered types requested.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is useful if you've registered multiple types with the same
        /// <see cref="Type"/> but different names.
        /// </para>
        /// <para>
        /// Be aware that this method does NOT return an instance for the default (unnamed) registration.
        /// </para>
        /// </remarks>
        /// <typeparam name="TResolvedType">The type requested.</typeparam>
        /// <param name="container">Container to resolve from.</param>
        /// <param name="resolverOverrides">Any overrides for the resolve calls.</param>
        /// <returns>Set of objects of type <typeparamref name="TResolvedType"/>.</returns>
        public static IEnumerable<TResolvedType> ResolveAll<TResolvedType>(this IUnityContainer container, params ResolverOverride[] resolverOverrides)
        {
            return (container ?? throw new ArgumentNullException(nameof(container))).ResolveAll(typeof(TResolvedType), resolverOverrides).Cast<TResolvedType>();
        }

        #endregion


        #region BuildUp overloads

        /// <summary>
        /// Run an existing object through the container and perform injection on it.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is useful when you don'registeredType control the construction of an
        /// instance (ASP.NET pages or objects created via XAML, for instance)
        /// but you still want properties and other injection performed.
        /// </para>
        /// <para>
        /// This overload uses the default registrations.
        /// </para>
        /// </remarks>
        /// <typeparam name="T"><see cref="Type"/> of object to perform injection on.</typeparam>
        /// <param name="container">Container to resolve through.</param>
        /// <param name="existing">Instance to build up.</param>
        /// <param name="resolverOverrides">Any overrides for the buildup.</param>
        /// <returns>The resulting object. By default, this will be <paramref name="existing"/>, but
        /// container extensions may add things like automatic proxy creation which would
        /// cause this to return a different object (but still type compatible with <typeparamref name="T"/>).</returns>
        public static T BuildUp<T>(this IUnityContainer container, T existing, params ResolverOverride[] resolverOverrides)
        {
            if (null == existing) throw new ArgumentNullException(nameof(existing));
            return (T)(container ?? throw new ArgumentNullException(nameof(container))).BuildUp(typeof(T), null, existing, resolverOverrides);
        }

        /// <summary>
        /// Run an existing object through the container and perform injection on it.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is useful when you don'registeredType control the construction of an
        /// instance (ASP.NET pages or objects created via XAML, for instance)
        /// but you still want properties and other injection performed.
        /// </para></remarks>
        /// <typeparam name="T"><see cref="Type"/> of object to perform injection on.</typeparam>
        /// <param name="container">Container to resolve through.</param>
        /// <param name="existing">Instance to build up.</param>
        /// <param name="name">name to use when looking up the type mappings and other configurations.</param>
        /// <param name="resolverOverrides">Any overrides for the Buildup.</param>
        /// <returns>The resulting object. By default, this will be <paramref name="existing"/>, but
        /// container extensions may add things like automatic proxy creation which would
        /// cause this to return a different object (but still type compatible with <typeparamref name="T"/>).</returns>
        public static T BuildUp<T>(this IUnityContainer container, T existing, string name, params ResolverOverride[] resolverOverrides)
        {
            if (null == existing) throw new ArgumentNullException(nameof(existing));
            return (T)(container ?? throw new ArgumentNullException(nameof(container))).BuildUp(typeof(T), name, existing, resolverOverrides);
        }

        /// <summary>
        /// Run an existing object through the container and perform injection on it.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is useful when you don'registeredType control the construction of an
        /// instance (ASP.NET pages or objects created via XAML, for instance)
        /// but you still want properties and other injection performed.
        /// </para>
        /// <para>
        /// This overload uses the default registrations.
        /// </para>
        /// </remarks>
        /// <param name="container">Container to resolve through.</param>
        /// <param name="type"><see cref="Type"/> of object to perform injection on.</param>
        /// <param name="existing">Instance to build up.</param>
        /// <param name="resolverOverrides">Any overrides for the Buildup.</param>
        /// <returns>The resulting object. By default, this will be <paramref name="existing"/>, but
        /// container extensions may add things like automatic proxy creation which would
        /// cause this to return a different object (but still type compatible with <paramref name="type"/>).</returns>
        public static object BuildUp(this IUnityContainer container, Type type, object existing, params ResolverOverride[] resolverOverrides)
        {
            return (container ?? throw new ArgumentNullException(nameof(container))).BuildUp(type, null, existing, resolverOverrides);
        }

        /// <summary>
        /// Run an existing object through the container and perform injection on it.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is useful when you don'registeredType control the construction of an
        /// instance (ASP.NET pages or objects created via XAML, for instance)
        /// but you still want properties and other injection performed.
        /// </para>
        /// <para>
        /// This overload uses the default registrations.
        /// </para>
        /// </remarks>
        /// <param name="container">Container to resolve through.</param>
        /// <param name="type"><see cref="Type"/> of object to perform injection on.</param>
        /// <param name="name">Registration name</param>
        /// <param name="existing">Instance to build up.</param>
        /// <param name="resolverOverrides">Any overrides for the Buildup.</param>
        /// <returns>The resulting object. By default, this will be <paramref name="existing"/>, but
        /// container extensions may add things like automatic proxy creation which would
        /// cause this to return a different object (but still type compatible with <paramref name="type"/>).</returns>
        public static object BuildUp(this IUnityContainer container, Type type, object existing, string name, params ResolverOverride[] resolverOverrides)
        {
            return (container ?? throw new ArgumentNullException(nameof(container))).BuildUp(type, name, existing, resolverOverrides);
        }


        #endregion


        #region Extension management and configuration

        /// <summary>
        /// Creates a new extension object and adds it to the container.
        /// </summary>
        /// <typeparam name="TExtension">Type of <see cref="UnityContainerExtension"/> to add. The extension type
        /// will be resolved from within the supplied <paramref name="container"/>.</typeparam>
        /// <param name="container">Container to add the extension to.</param>
        /// <returns>The <see cref="Unity.IUnityContainer"/> object that this method was called on (this in C#, Me in Visual Basic).</returns>
        public static IUnityContainer AddNewExtension<TExtension>(this IUnityContainer container)
            where TExtension : UnityContainerExtension
        {
            TExtension newExtension = (container ?? throw new ArgumentNullException(nameof(container))).Resolve<TExtension>();
            return container.AddExtension(newExtension);
        }

        /// <summary>
        /// ResolveMethod access to a configuration interface exposed by an extension.
        /// </summary>
        /// <remarks>Extensions can expose configuration interfaces as well as adding
        /// strategies and policies to the container. This method walks the list of
        /// added extensions and returns the first one that implements the requested type.
        /// </remarks>
        /// <typeparam name="TConfigurator">The configuration interface required.</typeparam>
        /// <param name="container">Container to configure.</param>
        /// <returns>The requested extension's configuration interface, or null if not found.</returns>
        public static TConfigurator Configure<TConfigurator>(this IUnityContainer container)
            where TConfigurator : IUnityContainerExtensionConfigurator
        {
            return (TConfigurator)(container ?? throw new ArgumentNullException(nameof(container))).Configure(typeof(TConfigurator));
        }

        #endregion


        #region Registration Helpers

        /// <summary>
        /// Check if a particular type has been registered with the container with
        /// the default name.
        /// </summary>
        /// <param name="container">Container to inspect.</param>
        /// <param name="typeToCheck">Type to check registration for.</param>
        /// <returns>True if this type has been registered, false if not.</returns>
        public static bool IsRegistered(this IUnityContainer container, Type typeToCheck)
        {
            return (container ?? throw new ArgumentNullException(nameof(container)))
                .IsRegistered(typeToCheck ?? throw new ArgumentNullException(nameof(typeToCheck)), null);
        }

        /// <summary>
        /// Check if a particular type has been registered with the container with the default name.
        /// </summary>
        /// <typeparam name="T">Type to check registration for.</typeparam>
        /// <param name="container">Container to inspect.</param>
        /// <returns>True if this type has been registered, false if not.</returns>
        public static bool IsRegistered<T>(this IUnityContainer container)
        {
            return (container ?? throw new ArgumentNullException(nameof(container))).IsRegistered(typeof(T), null);
        }

        /// <summary>
        /// Check if a particular type/name pair has been registered with the container.
        /// </summary>
        /// <typeparam name="T">Type to check registration for.</typeparam>
        /// <param name="container">Container to inspect.</param>
        /// <param name="nameToCheck">Name to check registration for.</param>
        /// <returns>True if this type/name pair has been registered, false if not.</returns>
        public static bool IsRegistered<T>(this IUnityContainer container, string nameToCheck)
        {
            return (container ?? throw new ArgumentNullException(nameof(container))).IsRegistered(typeof(T), nameToCheck);
        }

        #endregion
    }
}

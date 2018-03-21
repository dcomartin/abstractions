﻿// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Reflection;
using Unity.Build.Selected;
using Unity.Policy;
using Unity.Registration;
using Unity.Utility;

namespace Unity.Builder.Policy
{
    /// <summary>
    /// An implementation of <see cref="IConstructorSelectorPolicy"/> that selects
    /// the given constructor and creates the appropriate resolvers to call it with
    /// the specified parameters.
    /// </summary>
    public class SpecifiedConstructorSelectorPolicy : IConstructorSelectorPolicy
    {
        private readonly ConstructorInfo _ctor;
        private readonly InjectionParameterValue[] _parameterValues;

        /// <summary>
        /// Create an instance of <see cref="SpecifiedConstructorSelectorPolicy"/> that
        /// will return the given constructor, being passed the given injection values
        /// as parameters.
        /// </summary>
        /// <param name="ctor">The constructor to call.</param>
        /// <param name="parameterValues">Set of <see cref="InjectionParameterValue"/> objects
        /// that describes how to obtain the values for the constructor parameters.</param>
        public SpecifiedConstructorSelectorPolicy(ConstructorInfo ctor, InjectionParameterValue[] parameterValues)
        {
            _ctor = ctor;
            _parameterValues = parameterValues;
        }

        /// <summary>
        /// Choose the constructor to call for the given type.
        /// </summary>
        /// <param name="context">Current build context</param>
        /// <param name="resolverPolicyDestination">The <see cref='IPolicyList'/> to add any
        /// generated resolver objects into.</param>
        /// <returns>The chosen constructor.</returns>
        public SelectedConstructor SelectConstructor(IBuilderContext context, IPolicyList resolverPolicyDestination)
        {
            SelectedConstructor result;

            var typeInfo = (context ?? throw new ArgumentNullException(nameof(context))).BuildKey
                                                                                        .Type
                                                                                        .GetTypeInfo();
            var methodHasOpenGenericParameters = _ctor.GetParameters()
                                                      .Select(p => p.ParameterType.GetTypeInfo())
                                                      .Any(i => i.IsGenericType && i.ContainsGenericParameters);

            var ctorTypeInfo = _ctor.DeclaringType.GetTypeInfo();

            if (!methodHasOpenGenericParameters && !(ctorTypeInfo.IsGenericType && ctorTypeInfo.ContainsGenericParameters))
            {
                result = new SelectedConstructor(_ctor);
            }
            else
            {
                var closedCtorParameterTypes = _ctor.GetClosedParameterTypes(typeInfo.GenericTypeArguments);

                var constructor = typeInfo.DeclaredConstructors
                                          .Single(c => !c.IsStatic && c.GetParameters().ParametersMatch(closedCtorParameterTypes));

                result = new SelectedConstructor(constructor);
            }

            // TODO: 
            //foreach (var parameterValue in _parameterValues)
            //{
            //    var resolver = parameterValue.GetResolverPolicy(context.BuildKey.Type);
            //    result.AddParameterResolver(resolver);
            //}

            return result;
        }
    }
}

﻿// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.Build.Selected;
using Unity.Dependency;
using Unity.Policy;
using Unity.Registration;

namespace Unity.Builder.Policy
{
    /// <summary>
    /// An implementation of <see cref="IPropertySelectorPolicy"/> which returns
    /// the set of specific properties that the selector was configured with.
    /// </summary>
    public class SpecifiedPropertiesSelectorPolicy : IPropertySelectorPolicy
    {
        private readonly List<Tuple<PropertyInfo, InjectionParameterValue>> _propertiesAndValues =
            new List<Tuple<PropertyInfo, InjectionParameterValue>>();

        /// <summary>
        /// Add a property that will be par of the set returned when the 
        /// <see cref="SelectProperties(IBuilderContext, IPolicyList)"/> is called.
        /// </summary>
        /// <param name="property">The property to set.</param>
        /// <param name="value"><see cref="InjectionParameterValue"/> object describing
        /// how to create the value to inject.</param>
        public void AddPropertyAndValue(PropertyInfo property, InjectionParameterValue value)
        {
            _propertiesAndValues.Add(new Tuple<PropertyInfo, InjectionParameterValue>(property, value));
        }

        /// <summary>
        /// Returns sequence of properties on the given type that
        /// should be set as part of building that object.
        /// </summary>
        /// <param name="context">Current build context.</param>
        /// <param name="resolverPolicyDestination">The <see cref='IPolicyList'/> to add any
        /// generated resolver objects into.</param>
        /// <returns>Sequence of <see cref="PropertyInfo"/> objects
        /// that contain the properties to set.</returns>
        public IEnumerable<SelectedProperty> SelectProperties(IBuilderContext context, IPolicyList resolverPolicyDestination)
        {
            Type typeToBuild = context.BuildKey.Type;
            foreach (Tuple<PropertyInfo, InjectionParameterValue> pair in _propertiesAndValues)
            {
                var currentProperty = pair.Item1;
                var info = pair.Item1.DeclaringType.GetTypeInfo();

                // Is this the property info on the open generic? If so, get the one
                // for the current closed generic.
                if (info.IsGenericType && info.ContainsGenericParameters)
                {
                    currentProperty = context.BuildKey.Type.GetTypeInfo().GetDeclaredProperty(currentProperty.Name);
                }

                yield return new SelectedProperty(currentProperty, pair.Item2.GetResolverPolicy(typeToBuild));
            }
        }
    }
}

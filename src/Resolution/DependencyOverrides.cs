﻿// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;

namespace Unity.Resolution
{
    /// <summary>
    /// A convenience form of <see cref="DependencyOverride"/> that lets you
    /// specify multiple parameter overrides in one shot rather than having
    /// to construct multiple objects.
    /// </summary>
    /// <remarks>
    /// This class isn't really a collection, it just implements IEnumerable
    /// so that we get use of the nice C# collection initializer syntax.
    /// </remarks>
    public class DependencyOverrides : OverrideCollection<DependencyOverride, Type, object>
    {
        /// <summary>
        /// When implemented in derived classes, this pipeline is called from the <see cref="OverrideCollection{TOverride,TKey,TValue}.Add"/>
        /// pipeline to create the actual <see cref="ResolverOverride"/> objects.
        /// </summary>
        /// <param name="key">Key value to create the resolver.</param>
        /// <param name="value">Value to store in the resolver.</param>
        /// <returns>The created <see cref="ResolverOverride"/>.</returns>
        protected override DependencyOverride MakeOverride(Type key, object value)
        {
            return new DependencyOverride(key, value);
        }
    }
}

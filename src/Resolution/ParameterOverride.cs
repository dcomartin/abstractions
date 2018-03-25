﻿using System;
using Unity.Builder;
using Unity.Builder.Operation;
using Unity.Policy;

namespace Unity.Resolution
{
    /// <summary>
    /// A <see cref="ResolverOverride"/> class that lets you
    /// override a named parameter passed to a constructor.
    /// </summary>
    public class ParameterOverride : ResolverOverride
    {
        /// <summary>
        /// Construct a new <see cref="ParameterOverride"/> object that will
        /// override the given named constructor parameter, and pass the given
        /// value.
        /// </summary>
        /// <param name="parameterName">Name of the constructor parameter.</param>
        /// <param name="parameterValue">Value to pass for the constructor.</param>
        public ParameterOverride(string parameterName, object parameterValue)
            : base(parameterName, parameterValue ?? throw new ArgumentNullException(nameof(parameterValue)))
        {
        }

        /// <summary>
        /// Return a <see cref="IResolverPolicy"/> that can be used to give a value
        /// for the given desired dependency.
        /// </summary>
        /// <param name="context">Current build context.</param>
        /// <param name="dependencyType">Type of dependency desired.</param>
        /// <returns>a <see cref="IResolverPolicy"/> object if this override applies, null if not.</returns>
        public override IResolverPolicy GetResolver(IBuilderContext context, Type dependencyType)
        {
            throw new NotImplementedException(); // TODO: Fix
            //if ((context ?? throw new ArgumentNullException(nameof(context))).CurrentOperation is ParameterResolveOperation currentOperation &&
            //    currentOperation.ParameterName == Name)
            //{
            //    return Value.GetResolverPolicy(dependencyType);
            //}

            //return null;
        }
    }
}

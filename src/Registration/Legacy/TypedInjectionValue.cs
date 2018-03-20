using System;

namespace Unity.Registration
{
    /// <summary>
    /// A base class for implementing <see cref="InjectionParameterValue"/> classes
    /// that deal in explicit types.
    /// </summary>
    // TODO: [Obsolete]
    public class TypedInjectionValue : InjectionParameter
    {
        /// <summary>
        /// Create a new <see cref="TypedInjectionValue"/> that exposes
        /// information about the given <paramref name="parameterType"/>.
        /// </summary>
        /// <param name="parameterType">Type of the parameter.</param>
        protected TypedInjectionValue(Type parameterType, object value)
            : base(parameterType, value)
        {
        }
    }
}

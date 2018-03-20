using System;
using System.Collections.Generic;
using Unity.Policy;

namespace Unity.Registration
{
    /// <summary>
    /// Base type for objects that are used to configure parameters for
    /// constructor or method injection, or for getting the value to
    /// be injected into a property.
    /// </summary>
    // TODO: [Obsolete]
    public class InjectionParameterValue : InjectionParameter
    {
        protected InjectionParameterValue(object value)
            : base(value)
        {
        }

        /// <summary>
        /// Convert an arbitrary value to an InjectionParameterValue object. The rules are: 
        /// If it's already an InjectionParameterValue, return it. If it's a Type, return a
        /// ResolvedParameter object for that type. Otherwise return an InjectionParameter
        /// object for that value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The resulting <see cref="InjectionParameterValue"/>.</returns>
        public static InjectionParameter ToParameter(object value)
        {
            switch (value)
            {
                case InjectionParameterValue parameterValue:
                    return parameterValue;

                case Type typeValue:
                    return new ResolvedParameter(typeValue);
            }

            return new InjectionParameter(value);
        }
    }
}

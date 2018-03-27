using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using Unity.Build.Context;
using Unity.Build.Pipeline;
using Unity.Build.Policy;

namespace Unity.Registration
{
    /// <summary>
    /// A class that holds on to the given Parameter's value or info and provides
    /// the required resolver when the declaring type is instantiated.
    /// </summary>
    [DebuggerDisplay("InjectionParameter:  Type={ParameterType?.Name},  Value={ParameterValue}")]
    public class InjectionParameter : IResolve<ParameterInfo>
    {
        #region Constructors

        /// <summary>
        /// Create an instance of <see cref="InjectionParameter"/> that stores
        /// the given type of the parameter.
        /// </summary>
        /// <param name="type">Type to be injected for this parameter.</param>
        public InjectionParameter(Type type)
        {
            ParameterType = type ?? throw new ArgumentNullException(nameof(type));
            ParameterValue = type;
            Resolver = (ParameterInfo info) => (ref ResolutionContext context) => type;
            Expression = (ParameterInfo info) => throw new NotImplementedException();
        }

        /// <summary>
        /// Create an instance of <see cref="InjectionParameter"/> that stores
        /// the given value, using the runtime type of that value as the
        /// type of the parameter.
        /// </summary>
        /// <param name="value">Value to be injected for this parameter.</param>
        public InjectionParameter(object value)
        {
            ParameterValue = value;
            ParameterType = value is Type ? typeof(Type) : value?.GetType();
            Resolver = (ParameterInfo info) => (ref ResolutionContext context) => value;
            Expression = (ParameterInfo info) => throw new NotImplementedException();
        }

        /// <summary>
        /// Create an instance of <see cref="InjectionParameter"/> that stores
        /// the given value, associated with the given type.
        /// </summary>
        /// <param name="type">Type of the parameter.</param>
        /// <param name="value">Value of the parameter</param>
        public InjectionParameter(Type type, object value)
        {
            ParameterType = type ?? throw new ArgumentNullException(nameof(type));
            ParameterValue = value ?? throw new ArgumentNullException(nameof(value));
            Resolver = (ParameterInfo info) => (ref ResolutionContext context) => value;
            Expression = (ParameterInfo info) => throw new NotImplementedException();
        }

        #endregion


        #region Public Members

        /// <summary>
        /// The type of parameter this object represents.
        /// </summary>
        public virtual Type ParameterType { get; }

        /// <summary>
        /// The value of parameter this object represents.
        /// </summary>
        public virtual object ParameterValue { get; }

        #endregion


        #region IResolve

        public PipelineFactory<ParameterInfo, ResolveMethod> Resolver { get; }

        public PipelineFactory<ParameterInfo, Expression> Expression { get; }

        #endregion




        /// <summary>
        /// Test if this parameter matches type.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>True if this parameter value is compatible with type <paramref name="type"/></returns>
        public virtual bool MatchesType(Type type)
        {
            if (null == ParameterType) return true;

            var thisInfo = (typeof(Type) == ParameterType ? (Type)ParameterValue 
                                                          : ParameterType).GetTypeInfo();
            var typeInfo = type.GetTypeInfo();

            if (typeInfo.IsAssignableFrom(thisInfo)) return true;
            if ((thisInfo.IsArray || typeof(Array) == ParameterType || 
                typeof(Type) == ParameterType && typeof(Array).Equals(ParameterValue)) && 
                (typeInfo.IsArray || type == typeof(Array)))
                return true;

            if (thisInfo.IsGenericType && thisInfo.IsGenericTypeDefinition && 
                typeInfo.IsGenericType &&
                thisInfo.GetGenericTypeDefinition() == typeInfo.GetGenericTypeDefinition())
                return true;

            if (ParameterType == typeof(Type) && (Type)ParameterValue == type)
                return true;

            return false;
        }
    }

    /// <summary>
    /// A generic version of <see cref="InjectionParameter"/> that makes it a
    /// little easier to specify the type of the parameter.
    /// </summary>
    /// <typeparam name="TParameter">Type of parameter.</typeparam>
    public class InjectionParameter<TParameter> : InjectionParameter
    {
        public InjectionParameter()
            : base(typeof(TParameter))
        {
        }


        /// <summary>
        /// Create a new <see cref="InjectionParameter{TParameter}"/>.
        /// </summary>
        /// <param name="parameterValue">Value for the parameter.</param>
        public InjectionParameter(TParameter parameterValue)
            : base(typeof(TParameter), parameterValue)
        {
        }

        public new TParameter ParameterValue => (TParameter)base.ParameterValue;
    }
}

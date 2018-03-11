// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Unity.Attributes;
using Unity.Builder.Policy;
using Unity.Builder.Selection;
using Unity.Policy;
using Unity.Registration;
using Unity.Utility;

namespace Unity.Injection
{
    /// <summary>
    /// A class that holds the collection of information
    /// for a constructor, so that the container can
    /// be configured to call this constructor.
    /// </summary>
    public class InjectionConstructor : InjectionMember
    {
        private readonly InjectionParameterValue[] _data;
        private readonly Type[] _types;

        /// <summary>
        /// Create a new instance of <see cref="InjectionConstructor"/> that looks
        /// for a default constructor.
        /// </summary>
        public InjectionConstructor()
        {
        }

        /// <summary>
        /// Create a new instance of <see cref="InjectionConstructor"/> that looks
        /// for a constructor with the given set of parameter types.
        /// </summary>
        /// <param name="types">The types of the parameters of the constructor.</param>
        public InjectionConstructor(params Type[] types)
        {
            _types = types ?? throw new ArgumentNullException(nameof(types));
        }

        /// <summary>
        /// Create a new instance of <see cref="InjectionConstructor"/> that looks
        /// for a constructor with the given set of parameters.
        /// </summary>
        /// <param name="parameterValues">The values for the parameters, that will
        /// be converted to <see cref="InjectionParameterValue"/> objects.</param>
        public InjectionConstructor(params object[] parameterValues)
        {
            _data = (parameterValues ?? throw new ArgumentNullException(nameof(parameterValues)))
                .Select(InjectionParameterValue.ToParameter)
                .ToArray();
        }


        #region Legacy

        /// <summary>
        /// Add policies to the <paramref name="policies"/> to configure the
        /// container to call this constructor with the appropriate parameter values.
        /// </summary>
        /// <param name="serviceType">Interface registered, ignored in this implementation.</param>
        /// <param name="implementationType">Type to register.</param>
        /// <param name="name">Name used to resolve the type object.</param>
        /// <param name="policies">Policy list to add policies to.</param>
        public override void AddPolicies(Type serviceType, Type implementationType, string name, IPolicyList policies)
        {
            var policy = null != _data  ? ConstructorByArguments(implementationType, _data) :
                         null != _types ? ConstructorByType(implementationType, _types)     :
                                          DefaultConstructor(implementationType);

            policies.Set(serviceType, name, typeof(IConstructorSelectorPolicy), policy);
        }

        public override bool BuildRequired => true;

        private SpecifiedConstructorSelectorPolicy DefaultConstructor(Type typeToCreate)
        {
            foreach (var ctor in typeToCreate.GetTypeInfo()
                                             .DeclaredConstructors
                                             .Where(c => c.IsStatic == false && c.IsPublic))
            {
                if (!ctor.GetParameters().Select(p => p.ParameterType).Any())
                {
                    return new SpecifiedConstructorSelectorPolicy(ctor, new InjectionParameterValue[0]);
                }
            }

            throw new InvalidOperationException(
                string.Format(CultureInfo.CurrentCulture,
                    Constants.NoSuchConstructor,
                    typeToCreate.FullName, string.Empty));
        }


        private SpecifiedConstructorSelectorPolicy ConstructorByArguments(Type typeToCreate, InjectionParameterValue[] data)
        {
            foreach (var ctor in typeToCreate.GetTypeInfo()
                .DeclaredConstructors
                .Where(c => c.IsStatic == false && c.IsPublic))
            {
                if (_data.Matches(ctor.GetParameters().Select(p => p.ParameterType)))
                {
                    return new SpecifiedConstructorSelectorPolicy(ctor, data); 
                }
            }

            string signature = string.Join(", ", data.Select(p => p.ParameterTypeName).ToArray());

            throw new InvalidOperationException(
                string.Format(CultureInfo.CurrentCulture,
                    Constants.NoSuchConstructor,
                    typeToCreate.FullName,
                    signature));
        }

        private SpecifiedConstructorSelectorPolicy ConstructorByType(Type typeToCreate, Type[] types)
        {
            foreach (var ctor in typeToCreate.GetTypeInfo()
                                             .DeclaredConstructors
                                             .Where(c => c.IsStatic == false && c.IsPublic))
            {
                var parameters = ctor.GetParameters();
                if (parameters.ParametersMatch(types))
                {
                    return new SpecifiedConstructorSelectorPolicy(ctor, parameters.Select(ToResolvedParameter)
                                                                                  .ToArray());
                }
            }

            throw new InvalidOperationException(
                string.Format(CultureInfo.CurrentCulture, Constants.NoSuchConstructor, 
                typeToCreate.FullName, string.Join(", ", _types.Select(t => t.Name))));
        }

        private InjectionParameterValue ToResolvedParameter(ParameterInfo parameter)
        {
            return new ResolvedParameter(parameter.ParameterType, parameter.GetCustomAttributes(false)
                                                                           .OfType<DependencyAttribute>()
                                                                           .FirstOrDefault()?.Name);
        }

        #endregion


        #region Pipeline

        public override void AddPolicies(Type registeredType, string name, Type implementationType, IPolicySet policies)
        {
            var pipeline = null != _data  ? SelectByArgumentsConstructorPipeline(implementationType) :
                                            SelecByTypeConstructorPipeline(implementationType);

            policies.Set(typeof(SelectConstructorPipeline), pipeline);
        }

        private SelectConstructorPipeline SelecByTypeConstructorPipeline(Type type)
        {
            foreach (var ctor in type.GetTypeInfo().DeclaredConstructors)
            {
                if (ctor.IsStatic || !ctor.IsPublic || !Matches(_types, ctor.GetParameters()))
                    continue;

                var constructor = new SelectedConstructor(ctor);
                return (IUnityContainer c, Type t, string n) => constructor;
            }

            throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Constants.NoSuchConstructor,
                                                type.FullName, string.Join(", ", _types.Select(t => t.Name))));
        }

        private SelectConstructorPipeline SelectByArgumentsConstructorPipeline(Type type)
        {
            foreach (var ctor in type.GetTypeInfo().DeclaredConstructors)
            {
                if (ctor.IsStatic || !ctor.IsPublic || 
                    !Matches(_data.Cast<TypedInjectionValue>()
                                  .Select(d => d.ParameterType), 
                             ctor.GetParameters()))
                    continue;

                var constructor = new SelectedConstructor(ctor, _data);
                return (IUnityContainer c, Type t, string n) => constructor;
            }

            string signature = string.Join(", ", _data.Select(p => p.ParameterTypeName).ToArray());
            throw new InvalidOperationException( string.Format(CultureInfo.CurrentCulture,
                    Constants.NoSuchConstructor, type.FullName, signature));
        }

        #endregion


        #region Implementation

        private static bool Matches(IEnumerable<Type> types, IEnumerable<ParameterInfo> parameters)
        {
            if (null == types) return false;

            var enumerator1 = types.GetEnumerator();
            var enumerator2 = parameters.GetEnumerator();
            bool status1 = false;
            bool status2 = false;

            while ((status1 = enumerator1.MoveNext()) == (status2 = enumerator2.MoveNext()) && status1 && status2)
            {
                if (enumerator1.Current != enumerator2.Current.ParameterType)
                {
                    var info1 = enumerator1.Current.GetTypeInfo();
                    var info2 = enumerator2.Current.ParameterType.GetTypeInfo();

                    if ((info1.IsArray || enumerator1.Current == typeof(Array)) &&
                        (info2.IsArray || enumerator2.Current.ParameterType == typeof(Array)))
                        continue;

                    if (info1.IsGenericType && info2.IsGenericType &&
                        info1.GetGenericTypeDefinition() == info2.GetGenericTypeDefinition())
                        continue;


                    return false;
                }
            }

            return status1 == status2;
        }

        #endregion
    }
}

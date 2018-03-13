using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Unity.Dependency;
using Unity.Policy;

namespace Unity.Registration
{
    /// <summary>
    /// A class that holds the collection of information
    /// for a constructor, so that the container can
    /// be configured to call this constructor.
    /// </summary>
    public class InjectionConstructor : InjectionMemberWithParameters
    {
        /// <summary>
        /// Create a new instance of <see cref="InjectionConstructor"/> that looks
        /// for a default constructor.
        /// </summary>
        public InjectionConstructor()
            : base(0)
        {
        }

        /// <summary>
        /// Create a new instance of <see cref="InjectionConstructor"/> that looks
        /// for a constructor with the given set of parameter types.
        /// </summary>
        /// <param name="types">The types of the parameters of the constructor.</param>
        public InjectionConstructor(params Type[] types)
            : base((types ?? throw new ArgumentNullException(nameof(types))).Length)
        {
            for (var i = 0; i < types.Length; i++)
            {
                Parameters[i] = new InjectionParameter(types[i]);
            }
        }

        /// <summary>
        /// Create a new instance of <see cref="InjectionConstructor"/> that looks
        /// for a constructor with the given set of parameters.
        /// </summary>
        /// <param name="values">The values for the parameters, that will
        /// be converted to <see cref="InjectionParameterValue"/> objects.</param>
        public InjectionConstructor(params object[] values)
            : base((values ?? throw new ArgumentNullException(nameof(values))).Length)
        {
            for (var i = 0; i < values.Length; i++)
            {
                var value = values[i];
                Parameters[i] = value is InjectionParameter parameter 
                               ?  parameter :
                               new InjectionParameter(value);
            }
        }


        #region Legacy

        public override bool BuildRequired => true;

        #endregion


        #region Pipeline

        public override void AddPolicies(Type registeredType, string name, Type implementationType, IPolicySet policies)
        {
            var type = implementationType ?? registeredType;

            foreach (var ctor in type.GetTypeInfo().DeclaredConstructors)
            {
                if (ctor.IsStatic || !ctor.IsPublic || !Matches(ctor.GetParameters()))
                    continue;

                var constructor = new SelectedConstructor(ctor, Parameters);
                SelectConstructorPipeline pipeline = (IUnityContainer c, Type t, string n) => constructor;
                policies.Set(typeof(SelectConstructorPipeline), pipeline);
                return;
            }

            string signature = string.Join(", ", Parameters.Select(p => p.ParameterType?.Name).ToArray());
            throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                    Constants.NoSuchConstructor, type.FullName, signature));
        }

        #endregion
    }
}

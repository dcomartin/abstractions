using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Build.Pipeline;
using Unity.Build.Selected;
using Unity.Dependency;
using Unity.Policy;

namespace Unity.Registration
{
    /// <summary>
    /// A class that holds the collection of information
    /// for a constructor, so that the container can
    /// be configured to call this constructor.
    /// </summary>
    public class InjectionConstructor : InjectionMemberWithParameters<ConstructorInfo>
    {
        #region Error Constants

        protected override string NoMemberFound => Constants.NoSuchConstructor;
        // TODO: App proper error message
        protected override string MultipleFound { get; }

        #endregion



        #region Constructors

        /// <summary>
        /// Create a new instance of <see cref="InjectionConstructor"/> that looks
        /// for a default constructor.
        /// </summary>
        public InjectionConstructor() 
            : base()
        {
        }

        /// <summary>
        /// Create a new instance of <see cref="InjectionConstructor"/> that looks
        /// for a constructor with the given set of parameters.
        /// </summary>
        /// <param name="values">The values for the parameters, that will
        /// be converted to <see cref="InjectionParameterValue"/> objects.</param>
        public InjectionConstructor(params object[] values)
            : base(values)
        {
        }

        public InjectionConstructor(ConstructorInfo info)
        {
            MemberInfo = info;
        }

        #endregion


        #region Register Pipeline

        public override void AddPolicies(Type registeredType, string name, Type implementationType, IPolicySet policies)
        {
            var type = implementationType ?? registeredType;
            SelectedConstructor constructor = null;

            foreach (var ctor in type.GetTypeInfo().DeclaredConstructors)
            {
                if (ctor.IsStatic || !ctor.IsPublic || !Matches(ctor.GetParameters()))
                    continue;

                if (null != constructor)
                    throw new InvalidOperationException(MoreThanOneConstructor(type, constructor, ctor));

                constructor = 0 == Parameters.Length 
                            ? new SelectedConstructor(ctor)
                            : new SelectedConstructor(ctor, Parameters);
            }

            if (null == constructor)
                throw new InvalidOperationException(ErrorMessage(type, Constants.NoSuchConstructor));


            policies.Set(typeof(SelectedConstructor), constructor);
            
            // TODO: Remove
            SelectConstructor pipeline = (Type t) => constructor;
            policies.Set(typeof(SelectConstructor), pipeline);
        }

        #endregion


        #region Implementation

        protected override IEnumerable<ConstructorInfo> GetMemberInfos(Type type) => type.GetTypeInfo()
                                                                                     .DeclaredConstructors
                                                                                     .Where(c => !c.IsStatic && c.IsPublic);
        protected override ParameterInfo[] GetParameters(ConstructorInfo ctorInfo) => ctorInfo.GetParameters();

        #endregion


        private string MoreThanOneConstructor(Type type, SelectedConstructor constructor, ConstructorInfo ctor)
        {
            return ErrorMessage(type, $"The type {{0}} has multiple constructors {constructor.Constructor}, {ctor}, etc. satisfying signature ( {{1}} ). Unable to disambiguate.");
        }
    }
}

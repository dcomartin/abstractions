using System;
using Unity.Build.Pipeline;
using Unity.Lifetime;
using Unity.Policy;

namespace Unity.Build.Context
{
    public delegate ref ResolutionContext ParentDelegate();


    public struct ResolutionContext
    {
        public ILifetimeContainer LifetimeContainer;


        public IPolicySet Registration;

        public Type ImplementationType;

        public Type DeclaringType;


        public object Existing;



        public object Get(Type type, string name, Type policyInterface) => _get(type, name, policyInterface);

        public void Set(Type type, string name, Type policyInterface, object policy) => _set(type, name, policyInterface, policy);


        public ResolveDependency Resolve;


        #region Implementation

        public ResolutionContext(Func<Type, string, Type, object> getMethod, 
                               Action<Type, string, Type, object> setMethod)
        {
            LifetimeContainer = null;
            Registration = null;
            ImplementationType = null;
            DeclaringType = null;
            Existing = null;
            Resolve = null;

            _get = getMethod;
            _set = setMethod;
        }

        private Func<Type, string, Type, object> _get;
        private Action<Type, string, Type, object> _set;

        #endregion
    }
}

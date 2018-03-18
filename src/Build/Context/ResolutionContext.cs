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



        public object Get(Type policyInterface) => _get(policyInterface);

        public void Set(Type policyInterface, object policy) => _set(policyInterface, policy);


        public object Get(Type type, string name, Type policyInterface) => _getNamed(type, name, policyInterface);

        public void Set(Type type, string name, Type policyInterface, object policy) => _setNamed(type, name, policyInterface, policy);


        public ResolveDependency Resolve;


        #region Implementation

        public ResolutionContext(Func<Type, object> get, Func<Type, string, Type, object> getNamed, 
                                 Action<Type, object> set, Action<Type, string, Type, object> setNamed)
        {
            LifetimeContainer = null;
            Registration = null;
            ImplementationType = null;
            DeclaringType = null;
            Existing = null;
            Resolve = null;

            _get = get;
            _set = set;
            _getNamed = getNamed;
            _setNamed = setNamed;
        }

        private Func<Type, object> _get;
        private Action<Type, object> _set;
        private Func<Type, string, Type, object> _getNamed;
        private Action<Type, string, Type, object> _setNamed;

        #endregion
    }
}

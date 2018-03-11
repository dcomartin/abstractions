using System;
using Unity.Policy;

namespace Unity.Resolution
{
    public ref struct ResolutionContext
    {
        public IUnityContainer Container;

        public IPolicySet Registration;

        public GetRegistrationDelegate Get;

        public ResolverOverride[] Overrides;

        public Type Type;

        public object Target;

        public object Existing;

        public object Resolve(Type type, string name)
        {
            ResolutionContext context = new ResolutionContext
            {
                Container = Container,
                Registration = Get(type, name),
                Overrides = Overrides,
                Get = Get,

                Type = null,
                Existing = null,
                Target = null
            };

            return ((IResolve)context.Registration).Resolve(ref context);
        }
    }
}

using System;

namespace Unity.Build.Pipeline
{
    public delegate object GetPolicy(Type policyInterface);

    public delegate void SetPolicy(Type policyInterface, object policy);

    public delegate object GetDependencyPolicy(Type type, string name, Type policyInterface);

    public delegate void SetDependencyPolicy(Type type, string name, Type policyInterface, object policy);
}

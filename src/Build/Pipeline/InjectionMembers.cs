using System;
using System.Collections.Generic;
using Unity.Registration;

namespace Unity.Build.Pipeline
{
    public delegate IEnumerable<InjectionMember>   InjectionMembersPipeline(IUnityContainer container, Type type);
}

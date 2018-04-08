using System.Linq.Expressions;
using Unity.Build.Pipeline;

namespace Unity.Build.Policy
{
    public interface ITypeFactory<in TData>
    {
        Factory<TData, ResolveMethod> Activator { get; }

        Factory<TData, Expression> Expression { get; }
    }
}

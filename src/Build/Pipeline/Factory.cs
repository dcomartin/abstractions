using System.Collections.Generic;

namespace Unity.Build.Pipeline
{
    public delegate TPipeline Factory<in TData, out TPipeline>(TData data);


    public static class FactoryExtensions
    {
        public static T BuildPipeline<T>(this IList<Factory<T, T>> list)
        {
            T method = default(T);
            for (var i = list.Count - 1; i > -1; --i)
            {
                method = list[i](method);
            }
            return method;
        }
    }
}

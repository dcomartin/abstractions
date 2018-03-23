using System.Collections.Generic;

namespace Unity.Build.Factory
{
    public delegate T PipelineFactoryDelegate<T>(T next);


    public static class PipelineFactoryExtensions
    {
        public static T BuildPipeline<T>(this IList<PipelineFactoryDelegate<T>> list)
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

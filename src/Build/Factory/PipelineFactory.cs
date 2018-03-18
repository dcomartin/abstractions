using System.Collections.Generic;

namespace Unity.Build.Factory
{
    public delegate T PipelineFactoryDelegate<T>(T next);


    public static class PipelineFactoryExtensions
    {
        public static T BuildPipeline<T>(this IList<PipelineFactoryDelegate<T>> list)
        {
            T method = default(T);
            foreach (PipelineFactoryDelegate<T>  factory in list)
            {
                method = factory(method);
            }
            return method;
        }
    }
}

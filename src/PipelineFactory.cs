using System;
using System.Collections.Generic;
using System.Text;

namespace Unity
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

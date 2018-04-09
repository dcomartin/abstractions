using System;
using System.Linq.Expressions;
using Unity.Build.Policy;
using Unity.Container;
using Unity.Storage;

namespace Unity.Registration
{
    /// <summary>
    /// Base class for objects that can be used to configure what
    /// class members get injected by the container.
    /// </summary>
    public abstract class InjectionMember : ITypeFactory<Type>, IResolvePipeline
    {
        /// <summary>
        /// Allows injection member to inject necessary policies into registration
        /// </summary>
        /// <param name="registeredType">Registration type</param>
        /// <param name="name">Registration name</param>
        /// <param name="implementationType">Type of the implementation</param>
        /// <param name="set">Set where policies are kept</param>
        public virtual void AddPolicies(Type registeredType, string name, Type implementationType, IPolicySet set)
        {
            if (null != CreateExpression)  set.Set(typeof(Factory<Type, Expression>), CreateExpression);
            if (null != ResolveExpression) set.Set(typeof(Expression),                ResolveExpression);

            if (null != CreatePipeline)    set.Set(typeof(Factory<Type, ResolvePipeline>), CreatePipeline);
            if (null != ResolvePipeline)   set.Set(typeof(ResolvePipeline),                ResolvePipeline);
        }


        #region ITypeFactory

        public Factory<Type, ResolvePipeline> CreatePipeline { get; protected set; }

        public Factory<Type, Expression> CreateExpression { get; protected set; }

        #endregion


        #region IResolvePipeline

        public ResolvePipeline ResolvePipeline { get; protected set; }

        public Expression    ResolveExpression { get; protected set; }

        #endregion
    }
}

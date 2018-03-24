using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Unity.Build.Context;
using Unity.Build.Factory;
using Unity.Build.Pipeline;
using Unity.Registration;

namespace Unity.Build.Injection
{
    public abstract class InjectionMemberWithParameters<TMemberInfoType> : InjectionMember // TODO: Remove this 
                                                                                           //, IResolveMethodFactory<Type>
                                                  where TMemberInfoType  : MethodBase
    {
        #region Fields

        private TMemberInfoType   _info;
        private readonly object[] _data;

        #endregion


        #region Constructors

        protected InjectionMemberWithParameters()
        {
        }

        protected InjectionMemberWithParameters(object[] members)
        {
            _data = members;
        }

        #endregion


        #region Members

        protected TMemberInfoType MemberInfo
        {
            get => _info;
            set
            {
                Debug.Assert(null == _info, "Member Info could only be set once");

                _info = value ?? throw new InvalidOperationException("Member Info can not be null");

                Resolver = CreateResolverFactory();
            }
        }

        #endregion


        #region Type matching

        protected virtual bool Matches(ParameterInfo[] parameters)
        {
            // TODO: optimize
            if ((_data?.Length ?? 0) != parameters.Length) return false;

            for (var i = 0; i < (_data?.Length ?? 0); i++)
            {
                if (Matches(_data?[i], parameters[i].ParameterType))
                    continue;

                return false;
            }

            return true;
        }

        protected virtual bool Matches(object parameter, Type match)
        {
            switch (parameter)
            {
                case InjectionParameter injectionParameter:
                    return injectionParameter.MatchesType(match);

                case Type type:
                    return MatchesType(type, match);

                default:
                    return MatchesObject(parameter, match);
            }
        }

        public static bool MatchesType(Type type, Type match)
        {
            if (null == type) return true;

            var typeInfo = type.GetTypeInfo();
            var matchInfo = match.GetTypeInfo();

            if (matchInfo.IsAssignableFrom(typeInfo)) return true;
            if ((typeInfo.IsArray || typeof(Array) == type) &&
               (matchInfo.IsArray || match == typeof(Array)))
                return true;

            if (typeInfo.IsGenericType && typeInfo.IsGenericTypeDefinition && matchInfo.IsGenericType &&
                typeInfo.GetGenericTypeDefinition() == matchInfo.GetGenericTypeDefinition())
                return true;

            return false;
        }

        public static bool MatchesObject(object parameter, Type match)
        {
            var type = parameter is Type ? typeof(Type) : parameter?.GetType();

            if (null == type) return true;

            var typeInfo = type.GetTypeInfo();
            var matchInfo = match.GetTypeInfo();

            if (matchInfo.IsAssignableFrom(typeInfo)) return true;
            if ((typeInfo.IsArray || typeof(Array) == type) &&
                (matchInfo.IsArray || match == typeof(Array)))
                return true;

            if (typeInfo.IsGenericType && typeInfo.IsGenericTypeDefinition && matchInfo.IsGenericType &&
                typeInfo.GetGenericTypeDefinition() == matchInfo.GetGenericTypeDefinition())
                return true;

            return false;
        }

        #endregion


        #region Implementation

        protected virtual PipelineFactory<Type, ResolveMethod> CreateResolverFactory()
        {
            var parameters = _info.GetParameters();
            var length = parameters.Length;

            if (0 == length)
            {
                return type => null;
            }
            else
            {
                var factories = new PipelineFactory<Type, ResolveMethod>[length];

                if (null == _data || 0 == _data.Length)
                    for (var f = 0; f < length; f++) factories[f] = parameters[f].ToFactory();
                else
                {
                    Debug.Assert(length == _data.Length, "Number of InjectionMembers and parameters are different.");
                    for (var f = 0; f < length; f++) factories[f] = parameters[f].ToFactory(_data[f]);
                }

                return type =>
                {
                    var resolvers = new ResolveMethod[length];
                    for (var p = 0; p < length; p++) resolvers[p] = factories[p](type);

                    return (ref ResolutionContext context) =>
                    {
                        var values = new object[length];
                        for (var v = 0; v < length; v++) values[v] = resolvers[v](ref context);
                        return values;
                    };
                };
            }
        }


        // TODO: Optimize it out
        protected string ErrorMessage(Type type, string format)
        {
            string signature = string.Join(", ", _data.Select(p => p?.ToString() ?? "null"));
            return string.Format(CultureInfo.CurrentCulture, format, type.Name, signature);
        }

        #endregion
    }
}

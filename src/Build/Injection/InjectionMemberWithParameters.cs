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
    public abstract class InjectionMemberWithParameters<TMemberInfoType> : InjectionMember, 
                                                                           IResolveMethodFactory<Type>
                                                  where TMemberInfoType  : MethodBase
    {
        #region Fields

        protected object[] Parameters { get; } // TODO: Name properly
        private TMemberInfoType _info;

        #endregion


        #region Constructors

        protected InjectionMemberWithParameters()
        {
        }

        protected InjectionMemberWithParameters(object[] members)
        {
            Parameters = members;
        }

        protected InjectionMemberWithParameters(TMemberInfoType memberInfo)
        {
            MemberInfo = memberInfo;
        }

        protected InjectionMemberWithParameters(TMemberInfoType memberInfo, object[] members)
        {
            Parameters = members;
            MemberInfo = memberInfo;
        }

        #endregion


        #region Member Info

        protected TMemberInfoType MemberInfo
        {
            get => _info;
            set
            {
                Debug.Assert(null == _info, "Member Info could only be set once");
                _info = value;

                ParameterInfo[] parameters = _info.GetParameters();

                var length = parameters.Length;
                if (0 == length)
                {
                    var array = new object[0];
                    ResolveMethodFactory = type => (ref ResolutionContext context) => array;
                }
                else
                {
                    var factories = new ResolveMethodFactory<Type>[length];

                    if (null == Parameters || 0 == Parameters.Length)
                        for (var f = 0; f < length; f++) factories[f] = parameters[f].ToFactory();
                    else
                    {
                        Debug.Assert(length == Parameters.Length, "Number of InjectionMembers and paremeters are different.");
                        for (var f = 0; f < length; f++) factories[f] = parameters[f].ToFactory(Parameters[f]);
                    }

                    ResolveMethodFactory = type =>
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
        }

        #endregion


        #region IResolveMethodFactory

        public virtual ResolveMethodFactory<Type> ResolveMethodFactory { get; private set; }

        #endregion


        #region Type matching

        protected virtual bool Matches(ParameterInfo[] parameters)
        {
            // TODO: optimize
            if ((Parameters?.Length ?? 0) != parameters.Length) return false;

            for (var i = 0; i < (Parameters?.Length ?? 0); i++)
            {
                if (Matches(Parameters?[i], parameters[i].ParameterType))
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

        protected string ErrorMessage(Type type, string format)
        {
            string signature = string.Join(", ", Parameters.Select(p => p?.ToString() ?? "null"));
            return string.Format(CultureInfo.CurrentCulture, format, type.Name, signature);
        }

        #endregion
    }
}

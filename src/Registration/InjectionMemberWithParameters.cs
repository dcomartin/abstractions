using System.Globalization;
using System;
using System.Linq;
using System.Reflection;
using Unity.Registration;
using Unity.Policy;
using System.Collections.Generic;

namespace Unity.Dependency
{
    public abstract class InjectionMemberWithParameters<TMemberInfoType> : InjectionMember
    {
        #region Error Constants

        protected abstract string NoMemberFound { get; }

        protected abstract string MultipleFound { get; }

        #endregion


        #region Fields

        protected TMemberInfoType MemberInfo;
        protected readonly object[] Parameters;

        #endregion


        #region Constructors

        protected InjectionMemberWithParameters()
        {
            Parameters = new object[0];
        }

        protected InjectionMemberWithParameters(params object[] values)
        {
            Parameters = values ?? new object[0];
        }

        #endregion


        #region InjectionMember

        public override void AddPolicies(Type registeredType, string name, Type implementationType, IPolicySet policies)
        {
            var type = implementationType ?? registeredType;
            TMemberInfoType selection = default;

            foreach (var info in GetMemberInfos(type))
            {
                if (!Matches(GetParameters(info))) continue;

                //if (null != selection)
                //    throw new InvalidOperationException(MoreThanOneConstructor(type, selection, member));

                selection = info;
            }

            //if (null == selection)
            //    throw new InvalidOperationException(ErrorMessage(type, Constants.NoSuchConstructor));

            //policies.Set(typeof(SelectedConstructor), selection);

            //// TODO: Remove
            //SelectConstructor pipeline = (Type t) => selection;
            //policies.Set(typeof(SelectConstructor), pipeline);


            //base.AddPolicies(registeredType, name, implementationType, policies);
        }

        #endregion


        #region Type matching

        protected virtual bool Matches(ParameterInfo[] parameters)
        {
            if (Parameters.Length != parameters.Length) return false;

            for (var i = 0; i < Parameters.Length; i++)
            {
                if (Matches(Parameters[i], parameters[i].ParameterType))
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


        #endregion


        #region Implementation

        protected abstract IEnumerable<TMemberInfoType> GetMemberInfos(Type type);

        protected abstract ParameterInfo[] GetParameters(TMemberInfoType memberInfo);




        protected string ErrorMessage(Type type, string format)
        {
            string signature = string.Join(", ", Parameters.Select(p => p?.ToString() ?? "null"));
            return string.Format(CultureInfo.CurrentCulture, format, type.Name, signature);
        }

        public static bool MatchesType(Type type, Type match)
        {
            if (null == type) return true;

            var typeInfo = type.GetTypeInfo();
            var matchInfo = match.GetTypeInfo();

            if (matchInfo.IsAssignableFrom(typeInfo)) return true;
            if ((typeInfo.IsArray || typeof(Array).Equals(type)) &&
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
            if ((typeInfo.IsArray || typeof(Array).Equals(type)) &&
                (matchInfo.IsArray || match == typeof(Array)))
                return true;

            if (typeInfo.IsGenericType && typeInfo.IsGenericTypeDefinition && matchInfo.IsGenericType &&
                typeInfo.GetGenericTypeDefinition() == matchInfo.GetGenericTypeDefinition())
                return true;

            return false;
        }

        #endregion
    }
}

using System.Reflection;

namespace Unity.Registration
{
    public class InjectionMemberWithParameters : InjectionMember
    {
        protected readonly InjectionParameter[] Parameters;

        protected InjectionMemberWithParameters(int length)
        {
            Parameters = new InjectionParameter[length];
        }

        protected bool Matches(ParameterInfo[] parameters)
        {
            if (Parameters.Length != parameters.Length) return false;

            for (var i = 0; i < Parameters.Length; i++)
                if (!Parameters[i].MatchesType(parameters[i].ParameterType)) return false;

            return true;
        }
    }
}

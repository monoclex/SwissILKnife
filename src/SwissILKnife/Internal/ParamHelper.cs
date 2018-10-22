#if DEBUG && (NET45 || NET472)
#define DISKSAVING
#endif

using System.Reflection;

namespace SwissILKnife
{
	internal static class ParamHelper
	{
		public static bool IsByRef(this ParameterInfo parameterInfo)
			=> parameterInfo.ParameterType.IsByRef;

		public static bool IsOutOrRef(this ParameterInfo parameterInfo)
			=> parameterInfo.IsOut || parameterInfo.IsByRef();

		public static bool IsValueType(this ParameterInfo parameterInfo)
			=> parameterInfo.ParameterType.IsValueType;
	}
}
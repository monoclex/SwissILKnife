#if DEBUG && (NET45 || NET472)
#define DISKSAVING
#endif

namespace SwissILKnife
{
	public static class ParamHelper
	{
		public static bool IsByRef(this ParameterInfo parameterInfo)
			=> parameterInfo.ParameterType.IsByRef && !parameterInfo.IsOut;

		public static bool IsOutOrRef(this ParameterInfo parameterInfo)
			=> parameterInfo.ParameterType.IsByRef;

		public static bool IsOut(this ParameterInfo parameterInfo)
			=> parameterInfo.IsOut;

		public static bool IsValueType(this ParameterInfo parameterInfo)
			=> parameterInfo.ParameterType.IsValueType;
	}
}
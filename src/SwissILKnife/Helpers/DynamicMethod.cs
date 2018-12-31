using System;
using System.Reflection;
using System.Reflection.Emit;

namespace SwissILKnife
{
	public static class DynamicMethodExtensions
	{
		public static T CreateDelegate<T>(this DynamicMethod dynamicMethod)
			where T : Delegate
			=> (T)dynamicMethod.CreateDelegate(typeof(T));

		public static T CreateDelegate<T>(this DynamicMethod dynamicMethod, object target)
			where T : Delegate
			=> (T)dynamicMethod.CreateDelegate(typeof(T), target);

		public static DynamicMethod GetILGenerator(this DynamicMethod dyn, out ILGenerator ilGen)
		{
			ilGen = dyn.GetILGenerator();
			return dyn;
		}
	}
}
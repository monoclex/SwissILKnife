using System;
using System.Reflection;
using System.Reflection.Emit;

using EmitDynamicMethod = System.Reflection.Emit.DynamicMethod;

namespace SwissILKnife
{
	public class DynamicMethod<T>
		where T : Delegate
	{
		public EmitDynamicMethod EmitDynamicMethod { get; set; }
		public ILGenerator ILGenerator { get; set; }

		public Delegate CreateDelegate(Type delegateType)
			=> EmitDynamicMethod.CreateDelegate(delegateType);

		public Delegate CreateDelegate(Type delegateType, object target)
			=> EmitDynamicMethod.CreateDelegate(delegateType, target);

		private static readonly Type Type = typeof(T);

		public T CreateDelegate()
			=> (T)CreateDelegate(Type);

		private DynamicMethod(EmitDynamicMethod dynMethod)
		{
			EmitDynamicMethod = dynMethod;
			ILGenerator = dynMethod.GetILGenerator();
		}

		public DynamicMethod(string name, Type returnType, Type[] parameterTypes)
			: this(new EmitDynamicMethod(name, returnType, parameterTypes)) { }

		public DynamicMethod(string name, Type returnType, Type[] parameterTypes, bool restrictedSkipVisibility)
			: this(new EmitDynamicMethod(name, returnType, parameterTypes, restrictedSkipVisibility)) { }

		public DynamicMethod(string name, Type returnType, Type[] parameterTypes, Module m)
			: this(new EmitDynamicMethod(name, returnType, parameterTypes, m)) { }

		public DynamicMethod(string name, Type returnType, Type[] parameterTypes, Type owner)
			: this(new EmitDynamicMethod(name, returnType, parameterTypes, owner)) { }

		public DynamicMethod(string name, Type returnType, Type[] parameterTypes, Module m, bool skipVisibility)
			: this(new EmitDynamicMethod(name, returnType, parameterTypes, m, skipVisibility)) { }

		public DynamicMethod(string name, Type returnType, Type[] parameterTypes, Type owner, bool skipVisibility)
			: this(new EmitDynamicMethod(name, returnType, parameterTypes, owner, skipVisibility)) { }

		public DynamicMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, Module m, bool skipVisibility)
			: this(new EmitDynamicMethod(name, attributes, callingConvention, returnType, parameterTypes, m, skipVisibility)) { }

		public DynamicMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, Type owner, bool skipVisibility)
			: this(new EmitDynamicMethod(name, attributes, callingConvention, returnType, parameterTypes, owner, skipVisibility)) { }
	}

	internal static class DynamicMethodExtensions
	{
		public static DynamicMethod<T> GetILGenerator<T>(this DynamicMethod<T> dyn, out ILGenerator ilGen)
			where T : Delegate
		{
			ilGen = dyn.ILGenerator;
			return dyn;
		}
	}
}
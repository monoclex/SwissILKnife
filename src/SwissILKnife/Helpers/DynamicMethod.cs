using System;
using System.Reflection;
using System.Reflection.Emit;
using EmitDynamicMethod = System.Reflection.Emit.DynamicMethod;

namespace SwissILKnife
{
	public class DynamicMethod<T>
		where T : Delegate
	{
		private static readonly Type _type = typeof(T);

		public EmitDynamicMethod InnerDynamicMethod { get; set; }

		public DynamicMethod(EmitDynamicMethod dynMethod) => InnerDynamicMethod = dynMethod;

		public DynamicMethod(Type[] parameterTypes) : this(string.Empty, typeof(void), parameterTypes) { }
		public DynamicMethod(Type returnType, Type[] parameterTypes) : this(string.Empty, returnType, parameterTypes) { }

		public ILGenerator GetILGenerator() => InnerDynamicMethod.GetILGenerator();
		public ILGenerator GetILGenerator(int streamSize) => InnerDynamicMethod.GetILGenerator(streamSize);

		public T CreateDelegate() => (T)CreateDelegate(_type);
		public T CreateDelegate(object target) => (T)CreateDelegate(_type, target);

		public Delegate CreateDelegate(Type delegateType) => InnerDynamicMethod.CreateDelegate(delegateType);
		public Delegate CreateDelegate(Type delegateType, object target) => InnerDynamicMethod.CreateDelegate(delegateType, target);

		#region ctor noise
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
		#endregion
	}

	public static class DynamicMethodExtensions
	{
		public static DynamicMethod<T> GetILGenerator<T>(this DynamicMethod<T> dyn, out ILGenerator ilGen)
			where T : Delegate
		{
			ilGen = dyn.GetILGenerator();
			return dyn;
		}
	}
}
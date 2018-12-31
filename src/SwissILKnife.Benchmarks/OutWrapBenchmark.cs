using BenchmarkDotNet.Attributes;

using System;
using System.Reflection;
using System.Reflection.Emit;

namespace SwissILKnife.Benchmarks
{
	public class OutWrapBenchmark
	{
		public bool IntParse(string s, out int result)
			=> int.TryParse(s, out result);

		private MethodInfo _testMethod = typeof(OutWrapBenchmark).GetMethod(nameof(IntParse));

		private readonly Func<object, object[], object> _ilWrap;
		private readonly Wrapped _swissIlWrap;

		public OutWrapBenchmark()
		{
			_ilWrap = GenerateILWrap();
			_swissIlWrap = GenerateSwissILWrap();
		}

		[Benchmark]
		public Func<object, object[], object> GenerateILWrap()
			=> ILWrapRefSupport(_testMethod);

		[Benchmark]
		public Wrapped GenerateSwissILWrap()
			=> MethodWrapper.Wrap(_testMethod);

		[Benchmark]
		public bool CallILWrap()
			=> (bool)_ilWrap(this, new object[] { "5", null });

		[Benchmark]
		public bool CallSwissILWrap()
			=> (bool)_swissIlWrap(this, new object[] { "5", null });

		[Benchmark]
		public bool ReflectionInvoke()
			=> (bool)_testMethod.Invoke(this, new object[] { "5", null });

		public static Func<object, object[], object> ILWrapRefSupport(MethodInfo method)
		{
			var dm = new DynamicMethod(method.Name, typeof(object), new[] {
					typeof(object), typeof(object[])
				}, method.DeclaringType, true);
			var il = dm.GetILGenerator();

			if (!method.IsStatic)
			{
				il.Emit(OpCodes.Ldarg_0);
				il.Emit(OpCodes.Unbox_Any, method.DeclaringType);
			}

			var parameters = method.GetParameters();
			var locals = new LocalBuilder[parameters.Length];

			for (var i = 0; i < parameters.Length; i++)
			{
				if (!parameters[i].IsOut)
				{
					il.Emit(OpCodes.Ldarg_1);
					il.Emit(OpCodes.Ldc_I4, i);
					il.Emit(OpCodes.Ldelem_Ref);
				}

				var paramType = parameters[i].ParameterType;
				if (paramType.IsValueType)
					il.Emit(OpCodes.Unbox_Any, paramType);
			}

			for (var i = 0; i < parameters.Length; i++)
			{
				if (parameters[i].IsOut)
				{
					locals[i] = il.DeclareLocal(parameters[i].ParameterType.GetElementType());
					il.Emit(OpCodes.Ldloca, locals[locals.Length - 1]);
				}
			}

			il.EmitCall(method.IsStatic || method.DeclaringType.IsValueType ?
				OpCodes.Call : OpCodes.Callvirt, method, null);

			for (var idx = 0; idx < parameters.Length; ++idx)
			{
				if (parameters[idx].IsOut || parameters[idx].ParameterType.IsByRef)
				{
					il.Emit(OpCodes.Ldarg_1);
					il.Emit(OpCodes.Ldc_I4, idx);
					il.Emit(OpCodes.Ldloc, locals[idx].LocalIndex);

					if (parameters[idx].ParameterType.GetElementType().IsValueType)
						il.Emit(OpCodes.Box, parameters[idx].ParameterType.GetElementType());

					il.Emit(OpCodes.Stelem_Ref);
				}
			}

			if (method.ReturnType == null || method.ReturnType == typeof(void))
			{
				il.Emit(OpCodes.Ldnull);
			}
			else if (method.ReturnType.IsValueType)
			{
				il.Emit(OpCodes.Box, method.ReturnType);
			}

			il.Emit(OpCodes.Ret);
			return (Func<object, object[], object>)dm.CreateDelegate(typeof(Func<object, object[], object>));
		}
	}
}
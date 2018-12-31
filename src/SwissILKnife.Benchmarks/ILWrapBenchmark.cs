using BenchmarkDotNet.Attributes;

using System;
using System.Reflection;
using System.Reflection.Emit;

namespace SwissILKnife.Benchmarks
{
	public class ILWrapBenchmark
	{
		public string TestMethod(int a, int b, out int result, out int len)
		{
			var retresult = a.ToString() + b.ToString();
			result = a + b;
			len = retresult.Length;
			return retresult;
		}

		private MethodInfo _testMethod = typeof(ILWrapBenchmark).GetMethod(nameof(TestMethod));

		private readonly Func<object, object[], object> _ilWrap;
		private readonly Wrapped _swissIlWrap;

		public ILWrapBenchmark()
		{
			_ilWrap = GenerateILWrap();
			_swissIlWrap = GenerateSwissILWrap();
		}

		[Benchmark]
		public Func<object, object[], object> GenerateILWrap()
			=> ILWrap(_testMethod);

		[Benchmark]
		public Wrapped GenerateSwissILWrap()
			=> MethodWrapper.Wrap(_testMethod);

		[Benchmark]
		public string CallILWrap()
			=> (string)_ilWrap(this, new object[] { 5, 10, null, null });

		[Benchmark]
		public string CallSwissILWrap()
			=> (string)_swissIlWrap(this, new object[] { 5, 10, null, null });

		[Benchmark]
		public string ReflectionInvoke()
			=> (string)_testMethod.Invoke(this, new object[] { 5, 10, null, null });

		public static Func<object, object[], object> ILWrap(MethodInfo method)
		{
			var _object = typeof(object);

			var dm = new DynamicMethod(method.Name, _object, new[] {
					_object, typeof(object[])
				}, method.DeclaringType, true);
			var il = dm.GetILGenerator();

			if (!method.IsStatic)
			{
				il.Emit(OpCodes.Ldarg_0);
				il.Emit(OpCodes.Unbox_Any, method.DeclaringType);
			}
			var parameters = method.GetParameters();
			for (var i = 0; i < parameters.Length; i++)
			{
				il.Emit(OpCodes.Ldarg_1);
				il.Emit(OpCodes.Ldc_I4, i);
				il.Emit(OpCodes.Ldelem_Ref);
				il.Emit(OpCodes.Unbox_Any, parameters[i].ParameterType);
			}
			il.EmitCall(method.IsStatic || method.DeclaringType.IsValueType ?
				OpCodes.Call : OpCodes.Callvirt, method, null);
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
using BenchmarkDotNet.Attributes;

using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace SwissILKnife.Benchmarks
{
	public class SetComparisons
	{
		public string SomeProperty { get; set; }
		public string SomeField;

		private PropertyInfo _property;
		private FieldInfo _field;

		private readonly MethodInfo _propertySet;

		private readonly SetMethod _propSetExpressions;
		private readonly SetMethod _propSetIL;
		private readonly SetMethod _propSetSwissIL;
		private readonly SetMethod _fieldSetSwissIL;

		public SetComparisons()
		{
			_property = typeof(SetComparisons).GetProperty(nameof(SomeProperty), BindingFlags.Public | BindingFlags.Instance);
			_field = typeof(SetComparisons).GetField(nameof(SomeField), BindingFlags.Public | BindingFlags.Instance);

			_propertySet = _property.SetMethod;

			_propSetExpressions = PropCreateSetViaExpressions();
			_propSetIL = PropCreateSetViaIL();
			_propSetSwissIL = PropCreateSetViaSwissIL();
			_fieldSetSwissIL = FieldCreateSetViaSwissIL();

			_propSetSwissIL(this, string.Empty);
			_fieldSetSwissIL(this, string.Empty);
		}

		[Benchmark]
		public SetMethod PropCreateSetViaExpressions()
			=> SetViaExpression(_property);

		[Benchmark]
		public SetMethod PropCreateSetViaIL()
			=> SetViaIL(_propertySet);

		[Benchmark]
		public SetMethod PropCreateSetViaSwissIL()
			=> MemberUtils.GenerateSetMethod(_property);

		[Benchmark]
		public SetMethod FieldCreateSetViaSwissIL()
			=> MemberUtils.GenerateSetMethod(_field);

		[Benchmark]
		public void PropInvokeSetViaExpressions()
			=> _propSetExpressions(this, string.Empty);

		[Benchmark]
		public void PropInvokeSetViaIL()
			=> _propSetIL(this, string.Empty);

		[Benchmark]
		public void PropInvokeSetViaSwissIL()
			=> _propSetSwissIL(this, string.Empty);

		[Benchmark]
		public void FieldInvokeSetViaSwissIL()
			=> _fieldSetSwissIL(this, string.Empty);

		[Benchmark]
		public void PropReflectionSet()
			=> _property.SetValue(this, string.Empty);

		[Benchmark]
		public void FieldReflectionSet()
			=> _field.SetValue(this, string.Empty);

		private static SetMethod SetViaExpression(PropertyInfo info)
		{
			var setMethodInfo = info.GetSetMethod(true);
			var instance = Expression.Parameter(typeof(object), "instance");
			var value = Expression.Parameter(typeof(object), "value");
			var instanceCast = (!info.DeclaringType.GetTypeInfo().IsValueType) ? Expression.TypeAs(instance, info.DeclaringType) : Expression.Convert(instance, info.DeclaringType);
			var valueCast = (!info.PropertyType.GetTypeInfo().IsValueType) ? Expression.TypeAs(value, info.PropertyType) : Expression.Convert(value, info.PropertyType);

			return Expression.Lambda<SetMethod>(Expression.Call(instanceCast, setMethodInfo, valueCast), new ParameterExpression[] { instance, value }).Compile();
		}

		private static SetMethod SetViaIL(MethodInfo method)
		{
			var dm = new DynamicMethod(method.Name, null, new Type[] {
				typeof(object), typeof(object)
			}, method.DeclaringType, true);

			var il = dm.GetILGenerator();

			il.Emit(OpCodes.Ldarg_0);

			il.Emit(OpCodes.Ldarg_1);
			il.Emit(OpCodes.Unbox_Any, method.GetParameters()[0].ParameterType);

			il.EmitCall(
				method.IsStatic || method.DeclaringType.IsValueType ?
					OpCodes.Call
					: OpCodes.Callvirt,

				method,
				null);

			il.Emit(OpCodes.Ret);

			return (SetMethod)dm.CreateDelegate(typeof(SetMethod));
		}
	}
}
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace SwissILKnife.Benchmarks
{
	public class Img
	{
		// the code used for the banner image

		public Img()
		{
		}

		public void Code(PropertyInfo property)
		{
			property.SetValue(this, "abcd");

			var setMethodInfo = property.GetSetMethod(true);
			var instance = Expression.Parameter(typeof(object), "instance");
			var value = Expression.Parameter(typeof(object), "value");
			var instanceCast = (!property.DeclaringType.GetTypeInfo().IsValueType) ? Expression.TypeAs(instance, property.DeclaringType) : Expression.Convert(instance, property.DeclaringType);
			var valueCast = (!property.PropertyType.GetTypeInfo().IsValueType) ? Expression.TypeAs(value, property.PropertyType) : Expression.Convert(value, property.PropertyType);
			var expressionSet = Expression.Lambda<Action<object, object>>(Expression.Call(instanceCast, setMethodInfo, valueCast), new ParameterExpression[] { instance, value }).Compile();
			expressionSet(this, "abcd");

			var ilSet = MemberUtils.GetSetMethod(property);
			ilSet(this, "abcd");
		}
	}
}
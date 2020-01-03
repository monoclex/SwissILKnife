using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xunit;

namespace SwissILKnife.Tests.Issues
{
	// https://github.com/SirJosh3917/SwissILKnife/issues/2
	public class _2_ValueTuple_get_item_failing
	{
		[Fact]
		public void Test()
		{
			var tuple = ("Test", default(object));

			var field = tuple.GetType()
				.GetField(nameof(tuple.Item1), BindingFlags.Public | BindingFlags.Instance);

			var getter = MemberUtils.GenerateGetMethod(field);

			var testValue = getter(tuple);

			Assert.Equal(tuple.Item1, testValue);
		}

		[Fact]
		public void Exact()
		{
			(string, object) i = ("Test", null);
			var t = i.GetType();
			var f = t.GetField(nameof(i.Item1), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
			var gm = SwissILKnife.MemberUtils.GenerateGetMethod(f);
			var val = gm(i);

			Assert.Equal(i.Item1, val);
		}
	}
}

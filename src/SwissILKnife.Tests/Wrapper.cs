using System.Reflection;

using Xunit;

namespace SwissILKnife.Tests
{
	public class Wrapper
	{
		public readonly MethodInfo Example =
			typeof(Wrapper).GetMethod(nameof(ExampleWrap));

		public string ExampleWrap(int value) => value.ToString();

		[Fact]
		public void WrapsExample() => Assert.Equal("12345", MethodWrapper.Wrap(Example)(this, new object[] { 12345 }));

		public readonly MethodInfo ValueTypeExample =
			typeof(Wrapper).GetMethod(nameof(ValueTypeWrap));

		public int ValueTypeWrap(string value)
		{
			return int.TryParse(value, out var result) ?
				result
				: new int();
		}

		[Fact]
		public void WrapsValueType() => Assert.Equal(678, MethodWrapper.Wrap(ValueTypeExample)(this, new object[] { "678" }));

		public readonly MethodInfo OutFuncExample =
			typeof(Wrapper).GetMethod(nameof(OutFuncWrap));

		public bool OutFuncWrap(string input, out int result) => int.TryParse(input, out result);

		[Fact]
		public void WrapsOutFunc()
		{
			var args = new object[] { "1234", null };

			var exec = MethodWrapper.Wrap(OutFuncExample)
				(this, args);

			var success = (bool)(exec);

			Assert.True(success);
			Assert.Equal("1234", args[0]);
			Assert.Equal(1234, args[1]);
		}

		public readonly MethodInfo VoidExample =
			typeof(Wrapper).GetMethod(nameof(VoidWrap));

		public void VoidWrap()
		{
		}

		[Fact]
		public void WrapsVoidWrap()
		{
			var exec = MethodWrapper.Wrap(VoidExample)
				(this, null);

			Assert.Null(exec);
		}

		public static readonly MethodInfo StaticMethod =
			typeof(Wrapper).GetMethod(nameof(StaticWrap));

		public static string StaticWrap()
			=> ":)";

		[Fact]
		public void WrapsStaticFunction()
		{
			var exec = MethodWrapper.Wrap(StaticMethod)
				(null, null);

			Assert.Equal(":)", exec);
		}
	}
}
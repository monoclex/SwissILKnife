using System.Reflection;

using Xunit;

namespace SwissILKnife.Tests
{
	public class Sets
	{
		public readonly PropertyInfo LocalProperty
			= typeof(Sets).GetProperty(nameof(SomeLocalProperty));

		public readonly FieldInfo LocalField
			= typeof(Sets).GetField(nameof(SomeLocalField));

		public string SomeLocalProperty { get; set; }
		public string SomeLocalField;

		public static readonly PropertyInfo StaticProperty
			= typeof(Sets).GetProperty(nameof(SomeStaticProperty));

		public static readonly FieldInfo StaticField
			= typeof(Sets).GetField(nameof(SomeStaticField));

		public static string SomeStaticProperty { get; set; }
		public static string SomeStaticField;

		[Fact]
		public void SetsLocalProperty()
		{
			SomeLocalProperty = string.Empty;

			MemberUtils.GetSetMethod(LocalProperty)(this, " ");

			Assert.Equal(" ", SomeLocalProperty);
		}

		[Fact]
		public void SetsLocalField()
		{
			SomeLocalField = string.Empty;

			MemberUtils.GetSetMethod(LocalField)(this, " ");

			Assert.Equal(" ", SomeLocalField);
		}

		[Fact]
		public void SetsStaticProperty()
		{
			SomeStaticProperty = string.Empty;

			MemberUtils.GetSetMethod(StaticProperty)(this, " ");

			Assert.Equal(" ", SomeStaticProperty);
		}

		[Fact]
		public void SetsStaticField()
		{
			SomeStaticField = string.Empty;

			MemberUtils.GetSetMethod(StaticField)(this, " ");

			Assert.Equal(" ", SomeStaticField);
		}
	}
}
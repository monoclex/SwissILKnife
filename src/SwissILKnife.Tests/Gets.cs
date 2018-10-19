using System.Reflection;

using Xunit;

namespace SwissILKnife.Tests
{
	public class Gets
	{
		public readonly PropertyInfo LocalProperty
			= typeof(Gets).GetProperty(nameof(SomeLocalProperty));

		public readonly FieldInfo LocalField
			= typeof(Gets).GetField(nameof(SomeLocalField));

		public string SomeLocalProperty { get; set; }
		public string SomeLocalField;

		public static readonly PropertyInfo StaticProperty
			= typeof(Gets).GetProperty(nameof(SomeStaticProperty));

		public static readonly FieldInfo StaticField
			= typeof(Gets).GetField(nameof(SomeStaticField));

		public static string SomeStaticProperty { get; set; }
		public static string SomeStaticField;

		[Fact]
		public void GetsLocalProperty()
		{
			SomeLocalProperty = " ";

			Assert.Equal(" ", MemberUtils.GetGetMethod(LocalProperty)(this));
		}

		[Fact]
		public void GetsLocalField()
		{
			SomeLocalField = " ";

			Assert.Equal(" ", MemberUtils.GetGetMethod(LocalField)(this));
		}

		[Fact]
		public void GetsStaticProperty()
		{
			SomeStaticProperty = " ";

			Assert.Equal(" ", MemberUtils.GetGetMethod(StaticProperty)(null));
		}

		[Fact]
		public void GetsStaticField()
		{
			SomeStaticField = " ";

			Assert.Equal(" ", MemberUtils.GetGetMethod(StaticField)(null));
		}
	}
}
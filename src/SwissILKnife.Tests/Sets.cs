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

		public readonly PropertyInfo ValueTypeProperty
			= typeof(Sets).GetProperty(nameof(SomeValueTypeProperty));

		public static int SomeValueTypeProperty { get; set; }

		public readonly FieldInfo ValueTypeField
			= typeof(Sets).GetField(nameof(SomeValueTypeField));

		public static int SomeValueTypeField;

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

		[Fact]
		public void SetsValueTypeProperty()
		{
			SomeValueTypeProperty = 0;

			MemberUtils.GetSetMethod(ValueTypeProperty)(this, 1234);

			Assert.Equal(1234, SomeValueTypeProperty);
		}

		[Fact]
		public void SetsValueTypeField()
		{
			SomeValueTypeField = 0;

			MemberUtils.GetSetMethod(ValueTypeField)(this, 1234);

			Assert.Equal(1234, SomeValueTypeField);
		}
	}
}
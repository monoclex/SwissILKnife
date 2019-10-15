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

		public readonly PropertyInfo ValueTypeProperty
			= typeof(Gets).GetProperty(nameof(SomeValueTypeProperty));

		public static int SomeValueTypeProperty { get; set; }

		public readonly FieldInfo ValueTypeField
			= typeof(Gets).GetField(nameof(SomeValueTypeField));

		public static int SomeValueTypeField;

		[Fact]
		public void GetsLocalProperty()
		{
			SomeLocalProperty = " ";

			Assert.Equal(" ", MemberUtils.GenerateGetMethod(LocalProperty)(this));
		}

		[Fact]
		public void GetsLocalField()
		{
			SomeLocalField = " ";

			Assert.Equal(" ", MemberUtils.GenerateGetMethod(LocalField)(this));
		}

		[Fact]
		public void GetsStaticProperty()
		{
			SomeStaticProperty = " ";

			Assert.Equal(" ", MemberUtils.GenerateGetMethod(StaticProperty)(null));
		}

		[Fact]
		public void GetsStaticField()
		{
			SomeStaticField = " ";

			Assert.Equal(" ", MemberUtils.GenerateGetMethod(StaticField)(null));
		}

		[Fact]
		public void GetsValueTypeProperty()
		{
			SomeValueTypeProperty = 1234;

			Assert.Equal(1234, (int)MemberUtils.GenerateGetMethod(ValueTypeProperty)(null));
		}

		[Fact]
		public void GetsValueTypeField()
		{
			SomeValueTypeField = 1234;

			Assert.Equal(1234, (int)MemberUtils.GenerateGetMethod(ValueTypeField)(null));
		}
	}

	public class GetsValueTypeTests
	{
		private GetsValueType _valueType = GetsValueType.Initialize();

		[Fact]
		public void GetsLocalProperty()
		{
			_valueType.SomeLocalProperty = " ";

			Assert.Equal(" ", MemberUtils.GenerateGetMethod(_valueType.LocalProperty)(_valueType));
		}

		[Fact]
		public void GetsLocalField()
		{
			_valueType.SomeLocalField = " ";

			Assert.Equal(" ", MemberUtils.GenerateGetMethod(_valueType.LocalField)(_valueType));
		}

		[Fact]
		public void GetsStaticProperty()
		{
			GetsValueType.SomeStaticProperty = " ";

			Assert.Equal(" ", MemberUtils.GenerateGetMethod(GetsValueType.StaticProperty)(null));
		}

		[Fact]
		public void GetsStaticField()
		{
			GetsValueType.SomeStaticField = " ";

			Assert.Equal(" ", MemberUtils.GenerateGetMethod(GetsValueType.StaticField)(null));
		}

		[Fact]
		public void GetsValueTypeProperty()
		{
			GetsValueType.SomeValueTypeProperty = 1234;

			Assert.Equal(1234, (int)MemberUtils.GenerateGetMethod(_valueType.ValueTypeProperty)(null));
		}

		[Fact]
		public void GetsValueTypeField()
		{
			GetsValueType.SomeValueTypeField = 1234;

			Assert.Equal(1234, (int)MemberUtils.GenerateGetMethod(_valueType.ValueTypeField)(null));
		}
	}

	public struct GetsValueType
	{
		public static GetsValueType Initialize()
		{
			var o = new GetsValueType
			(
				typeof(GetsValueType).GetProperty(nameof(SomeLocalProperty)),
				typeof(GetsValueType).GetField(nameof(SomeLocalField)),
				typeof(GetsValueType).GetProperty(nameof(SomeValueTypeProperty)),
				typeof(GetsValueType).GetField(nameof(SomeValueTypeField))
			);

			return o;
		}

		public GetsValueType
		(
			PropertyInfo localProperty,
			FieldInfo localField,
			PropertyInfo valueTypeProperty,
			FieldInfo valueTypeField
		)
		{
			LocalProperty = localProperty;
			LocalField = localField;

			SomeLocalField = default;
			SomeLocalProperty = default;
			ValueTypeProperty = valueTypeProperty;
			ValueTypeField = valueTypeField;
		}

		public readonly PropertyInfo LocalProperty;

		public readonly FieldInfo LocalField;

		public string SomeLocalProperty { get; set; }
		public string SomeLocalField;

		public static readonly PropertyInfo StaticProperty
			= typeof(Gets).GetProperty(nameof(SomeStaticProperty));

		public static readonly FieldInfo StaticField
			= typeof(Gets).GetField(nameof(SomeStaticField));

		public static string SomeStaticProperty { get; set; }
		public static string SomeStaticField;

		public readonly PropertyInfo ValueTypeProperty;

		public static int SomeValueTypeProperty { get; set; }

		public readonly FieldInfo ValueTypeField;

		public static int SomeValueTypeField;
	}
}
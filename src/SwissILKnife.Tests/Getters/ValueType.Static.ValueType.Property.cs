using System.Reflection;
using Xunit;

namespace SwissILKnife.Tests.Getters
{
	public partial struct ValueType
	{
		public static PropertyInfo PropertyInfo_Static_ValueType_Property
			=> Property(nameof(Static_ValueType_Property), BindingFlags.Public | BindingFlags.Static);

		public static int Static_ValueType_Property { get; set; }
	}

	public partial class ValueTypeTests
	{
		[Fact]
		public void Static_ValueType_Property()
		{
			ValueType.Static_ValueType_Property = Magic_ValueType;

			var getter = MemberUtils.GenerateGetMethod(ValueType.PropertyInfo_Static_ValueType_Property);
			Assert.Equal(Magic_ValueType, getter(null));
		}
	}
}

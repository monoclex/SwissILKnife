using System.Reflection;
using Xunit;

namespace SwissILKnife.Tests.Setters
{
	public partial class ReferenceType
	{
		public static PropertyInfo PropertyInfo_Static_ValueType_Property
			=> Property(nameof(Static_ValueType_Property), BindingFlags.Public | BindingFlags.Static);

		public static int Static_ValueType_Property { get; set; }
	}

	public partial class ReferenceTypeTests
	{
		[Fact]
		public void Static_ValueType_Property()
		{
			MemberUtils.GenerateSetMethod(ReferenceType.PropertyInfo_Static_ValueType_Property)(null, Magic_ValueType);
			Assert.Equal(Magic_ValueType, ReferenceType.Static_ValueType_Property);
		}
	}
}

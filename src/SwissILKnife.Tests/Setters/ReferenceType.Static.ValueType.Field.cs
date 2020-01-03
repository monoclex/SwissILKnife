using System.Reflection;
using Xunit;

namespace SwissILKnife.Tests.Setters
{
	public partial class ReferenceType
	{
		public static FieldInfo PropertyInfo_Static_ValueType_Field
			=> Field(nameof(Static_ValueType_Field), BindingFlags.Public | BindingFlags.Static);

		public static int Static_ValueType_Field;
	}

	public partial class ReferenceTypeTests
	{
		[Fact]
		public void Static_ValueType_Field()
		{
			MemberUtils.GenerateSetMethod(ReferenceType.PropertyInfo_Static_ValueType_Field)(null, Magic_ValueType);
			Assert.Equal(Magic_ValueType, ReferenceType.Static_ValueType_Field);
		}
	}
}

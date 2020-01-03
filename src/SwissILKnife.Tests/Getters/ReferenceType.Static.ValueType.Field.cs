using System.Reflection;
using Xunit;

namespace SwissILKnife.Tests.Getters
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
			ReferenceType.Static_ValueType_Field = Magic_ValueType;

			var getter = MemberUtils.GenerateGetMethod(ReferenceType.PropertyInfo_Static_ValueType_Field);
			Assert.Equal(Magic_ValueType, getter(null));
		}
	}
}

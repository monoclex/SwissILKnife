using Xunit;

namespace SwissILKnife.Tests.Getters
{
	public partial class ValueTypeTests
	{
		[Fact]
		public void Static_ReferenceType_Field()
		{
			ValueType.Static_ReferenceType_Field = Magic_ReferenceType;

			var getter = MemberUtils.GenerateGetMethod(ValueType.FieldInfo_Static_ReferenceType_Field);
			Assert.Equal(Magic_ReferenceType, getter(null));
		}
	}
}
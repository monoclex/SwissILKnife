using Xunit;

namespace SwissILKnife.Tests.Getters
{
	public partial class ReferenceTypeTests
	{
		[Fact]
		public void Static_ReferenceType_Field()
		{
			ReferenceType.Static_ReferenceType_Field = Magic_ReferenceType;

			var getter = MemberUtils.GenerateGetMethod(ReferenceType.FieldInfo_Static_ReferenceType_Field);
			Assert.Equal(Magic_ReferenceType, getter(null));
		}
	}
}
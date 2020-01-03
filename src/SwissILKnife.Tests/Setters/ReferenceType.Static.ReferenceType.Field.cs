using Xunit;

namespace SwissILKnife.Tests.Setters
{
	public partial class ReferenceTypeTests
	{
		[Fact]
		public void Static_ReferenceType_Field()
		{
			MemberUtils.GenerateSetMethod(ReferenceType.FieldInfo_Static_ReferenceType_Field)(null, Magic_ReferenceType);
			Assert.Equal(Magic_ReferenceType, ReferenceType.Static_ReferenceType_Field);
		}
	}
}
using Xunit;

namespace SwissILKnife.Tests.Setters
{
	public partial class ReferenceTypeTests
	{
		[Fact]
		public void Static_ReferenceType_Property()
		{
			MemberUtils.GenerateSetMethod(ReferenceType.PropertyInfo_Static_ReferenceType_Property)(null, Magic_ReferenceType);
			Assert.Equal(Magic_ReferenceType, ReferenceType.Static_ReferenceType_Property);
		}
	}
}
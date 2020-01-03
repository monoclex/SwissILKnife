using Xunit;

namespace SwissILKnife.Tests.Getters
{
	public partial class ReferenceTypeTests
	{
		[Fact]
		public void Static_ReferenceType_Property()
		{
			ReferenceType.Static_ReferenceType_Property = Magic_ReferenceType;

			var getter = MemberUtils.GenerateGetMethod(ReferenceType.PropertyInfo_Static_ReferenceType_Property);
			Assert.Equal(Magic_ReferenceType, getter(null));
		}
	}
}
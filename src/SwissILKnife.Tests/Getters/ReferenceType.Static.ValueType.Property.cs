using Xunit;

namespace SwissILKnife.Tests.Getters
{
	public partial class ReferenceTypeTests
	{
		[Fact]
		public void Static_ValueType_Property()
		{
			ReferenceType.Static_ValueType_Property = Magic_ValueType;

			var getter = MemberUtils.GenerateGetMethod(ReferenceType.PropertyInfo_Static_ValueType_Property);
			Assert.Equal(Magic_ValueType, getter(null));
		}
	}
}
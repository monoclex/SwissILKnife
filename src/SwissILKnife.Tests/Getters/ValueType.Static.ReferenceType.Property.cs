using Xunit;

namespace SwissILKnife.Tests.Getters
{
	public partial class ValueTypeTests
	{
		[Fact]
		public void Static_ReferenceType_Property()
		{
			ValueType.Static_ReferenceType_Property = Magic_ReferenceType;

			var getter = MemberUtils.GenerateGetMethod(ValueType.PropertyInfo_Static_ReferenceType_Property);
			Assert.Equal(Magic_ReferenceType, getter(null));
		}
	}
}
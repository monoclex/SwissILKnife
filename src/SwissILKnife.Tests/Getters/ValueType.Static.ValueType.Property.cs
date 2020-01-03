using Xunit;

namespace SwissILKnife.Tests.Getters
{
	public partial class ValueTypeTests
	{
		[Fact]
		public void Static_ValueType_Property()
		{
			ValueType.Static_ValueType_Property = Magic_ValueType;

			var getter = MemberUtils.GenerateGetMethod(ValueType.PropertyInfo_Static_ValueType_Property);
			Assert.Equal(Magic_ValueType, getter(null));
		}
	}
}
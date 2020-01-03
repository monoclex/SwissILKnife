using Xunit;

namespace SwissILKnife.Tests.Getters
{
	public partial class ValueTypeTests
	{
		[Fact]
		public void Static_ValueType_Field()
		{
			ValueType.Static_ValueType_Field = Magic_ValueType;

			var getter = MemberUtils.GenerateGetMethod(ValueType.PropertyInfo_Static_ValueType_Field);
			Assert.Equal(Magic_ValueType, getter(null));
		}
	}
}
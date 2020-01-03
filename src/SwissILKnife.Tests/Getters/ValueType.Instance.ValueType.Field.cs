using Xunit;

namespace SwissILKnife.Tests.Getters
{
	public partial class ValueTypeTests
	{
		[Fact]
		public void Instance_ValueType_Field()
		{
			var instance = new ValueType();
			instance.Instance_ValueType_Field = Magic_ValueType;

			var getter = MemberUtils.GenerateGetMethod(ValueType.PropertyInfo_Instance_ValueType_Field);
			Assert.Equal(Magic_ValueType, getter(instance));
		}
	}
}
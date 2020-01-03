using Xunit;

namespace SwissILKnife.Tests.Getters
{
	public partial class ReferenceTypeTests
	{
		[Fact]
		public void Instance_ValueType_Property()
		{
			var instance = new ReferenceType();
			instance.Instance_ValueType_Property = Magic_ValueType;

			var getter = MemberUtils.GenerateGetMethod(ReferenceType.PropertyInfo_Instance_ValueType_Property);
			Assert.Equal(Magic_ValueType, getter(instance));
		}
	}
}
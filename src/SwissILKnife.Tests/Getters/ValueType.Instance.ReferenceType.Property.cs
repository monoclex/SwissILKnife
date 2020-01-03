using Xunit;

namespace SwissILKnife.Tests.Getters
{
	public partial class ValueTypeTests
	{
		[Fact]
		public void Instance_ReferenceType_Property()
		{
			var instance = new ValueType();
			instance.Instance_ReferenceType_Property = Magic_ReferenceType;

			var getter = MemberUtils.GenerateGetMethod(ValueType.PropertyInfo_Instance_ReferenceType_Property);
			Assert.Equal(Magic_ReferenceType, getter(instance));
		}
	}
}
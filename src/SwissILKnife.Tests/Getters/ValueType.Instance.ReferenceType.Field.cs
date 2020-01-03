using Xunit;

namespace SwissILKnife.Tests.Getters
{
	public partial class ValueTypeTests
	{
		[Fact]
		public void Instance_ReferenceType_Field()
		{
			var instance = new ValueType();
			instance.Instance_ReferenceType_Field = Magic_ReferenceType;

			var getter = MemberUtils.GenerateGetMethod(ValueType.PropertyInfo_Instance_ReferenceType_Field);
			Assert.Equal(Magic_ReferenceType, getter(instance));
		}
	}
}
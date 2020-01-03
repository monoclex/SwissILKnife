using Xunit;

namespace SwissILKnife.Tests.Getters
{
	public partial class ReferenceTypeTests
	{
		[Fact]
		public void Instance_ReferenceType_Property()
		{
			var instance = new ReferenceType();
			instance.Instance_ReferenceType_Property = Magic_ReferenceType;

			var getter = MemberUtils.GenerateGetMethod(ReferenceType.PropertyInfo_Instance_ReferenceType_Property);
			Assert.Equal(Magic_ReferenceType, getter(instance));
		}
	}
}
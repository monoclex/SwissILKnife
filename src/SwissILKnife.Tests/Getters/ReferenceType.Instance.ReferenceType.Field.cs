using Xunit;

namespace SwissILKnife.Tests.Getters
{
	public partial class ReferenceTypeTests
	{
		[Fact]
		public void Instance_ReferenceType_Field()
		{
			var instance = new ReferenceType();
			instance.Instance_ReferenceType_Field = Magic_ReferenceType;

			var getter = MemberUtils.GenerateGetMethod(ReferenceType.FieldInfo_Instance_ReferenceType_Field);
			Assert.Equal(Magic_ReferenceType, getter(instance));
		}
	}
}
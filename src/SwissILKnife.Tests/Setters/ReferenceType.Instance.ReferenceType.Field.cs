using Xunit;

namespace SwissILKnife.Tests.Setters
{
	public partial class ReferenceTypeTests
	{
		[Fact]
		public void Instance_ReferenceType_Field()
		{
			var instance = new ReferenceType();

			MemberUtils.GenerateSetMethod(ReferenceType.FieldInfo_Instance_ReferenceType_Field)(instance, Magic_ReferenceType);
			Assert.Equal(Magic_ReferenceType, instance.Instance_ReferenceType_Field);
		}
	}
}
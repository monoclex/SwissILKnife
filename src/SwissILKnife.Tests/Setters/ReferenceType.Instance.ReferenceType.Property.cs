using Xunit;

namespace SwissILKnife.Tests.Setters
{
	public partial class ReferenceTypeTests
	{
		[Fact]
		public void Instance_ReferenceType_Property()
		{
			var instance = new ReferenceType();

			MemberUtils.GenerateSetMethod(ReferenceType.PropertyInfo_Instance_ReferenceType_Property)(instance, Magic_ReferenceType);
			Assert.Equal(Magic_ReferenceType, instance.Instance_ReferenceType_Property);
		}
	}
}
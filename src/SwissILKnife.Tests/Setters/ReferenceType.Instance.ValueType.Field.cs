using Xunit;

namespace SwissILKnife.Tests.Setters
{
	public partial class ReferenceTypeTests
	{
		[Fact]
		public void Instance_ValueType_Field()
		{
			var instance = new ReferenceType();

			MemberUtils.GenerateSetMethod(ReferenceType.PropertyInfo_Instance_ValueType_Field)(instance, Magic_ValueType);
			Assert.Equal(Magic_ValueType, instance.Instance_ValueType_Field);
		}
	}
}
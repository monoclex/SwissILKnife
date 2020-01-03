using Xunit;

namespace SwissILKnife.Tests.Setters
{
	public partial class ReferenceTypeTests
	{
		[Fact]
		public void Instance_ValueType_Property()
		{
			var instance = new ReferenceType();

			MemberUtils.GenerateSetMethod(ReferenceType.PropertyInfo_Instance_ValueType_Property)(instance, Magic_ValueType);
			Assert.Equal(Magic_ValueType, instance.Instance_ValueType_Property);
		}
	}
}
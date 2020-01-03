using System.Reflection;
using Xunit;

namespace SwissILKnife.Tests.Getters
{
	public partial struct ValueType
	{
		public static PropertyInfo PropertyInfo_Instance_ValueType_Property
			=> Property(nameof(Instance_ValueType_Property), BindingFlags.Public | BindingFlags.Instance);

		public int Instance_ValueType_Property { get; set; }
	}

	public partial class ValueTypeTests
	{
		[Fact]
		public void Instance_ValueType_Property()
		{
			var instance = new ValueType();
			instance.Instance_ValueType_Property = Magic_ValueType;

			var getter = MemberUtils.GenerateGetMethod(ValueType.PropertyInfo_Instance_ValueType_Property);
			Assert.Equal(Magic_ValueType, getter(instance));
		}
	}
}

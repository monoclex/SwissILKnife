using System.Reflection;

namespace SwissILKnife.Tests
{
	public partial class ReferenceType
	{
		public static PropertyInfo PropertyInfo_Instance_ValueType_Property
			=> Property(nameof(Instance_ValueType_Property), BindingFlags.Public | BindingFlags.Instance);

		public int Instance_ValueType_Property { get; set; }
	}
}
using System.Reflection;

namespace SwissILKnife.Tests
{
	public partial class ReferenceType
	{
		public static PropertyInfo PropertyInfo_Static_ValueType_Property
			=> Property(nameof(Static_ValueType_Property), BindingFlags.Public | BindingFlags.Static);

		public static int Static_ValueType_Property { get; set; }
	}
}
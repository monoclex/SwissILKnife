using System.Reflection;

namespace SwissILKnife.Tests
{
	public partial class ReferenceType
	{
		public static FieldInfo PropertyInfo_Static_ValueType_Field
			=> Field(nameof(Static_ValueType_Field), BindingFlags.Public | BindingFlags.Static);

		public static int Static_ValueType_Field;
	}
}
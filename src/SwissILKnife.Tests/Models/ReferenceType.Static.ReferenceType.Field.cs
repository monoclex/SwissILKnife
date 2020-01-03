using System.Reflection;

namespace SwissILKnife.Tests
{
	public partial class ReferenceType
	{
		public static FieldInfo FieldInfo_Static_ReferenceType_Field
			=> Field(nameof(Static_ReferenceType_Field), BindingFlags.Public | BindingFlags.Static);

		public static string Static_ReferenceType_Field;
	}
}
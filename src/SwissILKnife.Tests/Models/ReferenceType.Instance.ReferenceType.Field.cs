using System.Reflection;

namespace SwissILKnife.Tests
{
	public partial class ReferenceType
	{
		public static FieldInfo FieldInfo_Instance_ReferenceType_Field
			=> Field(nameof(Instance_ReferenceType_Field), BindingFlags.Public | BindingFlags.Instance);

		public string Instance_ReferenceType_Field;
	}
}
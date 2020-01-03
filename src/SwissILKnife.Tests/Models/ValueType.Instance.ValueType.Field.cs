using System.Reflection;

namespace SwissILKnife.Tests
{
	public partial struct ValueType
	{
		public static FieldInfo PropertyInfo_Instance_ValueType_Field
			=> Field(nameof(Instance_ValueType_Field), BindingFlags.Public | BindingFlags.Instance);

		public int Instance_ValueType_Field;
	}
}
using System.Reflection;

namespace SwissILKnife.Tests
{
	public partial struct ValueType
	{
		private static PropertyInfo Property(string name, BindingFlags flags)
			=> typeof(ValueType).GetProperty(name, flags);

		private static FieldInfo Field(string name, BindingFlags flags)
			=> typeof(ValueType).GetField(name, flags);
	}
}
using System.Reflection;

namespace SwissILKnife.Tests
{
	public partial class ReferenceType
	{
		private static PropertyInfo Property(string name, BindingFlags flags)
			=> typeof(ReferenceType).GetProperty(name, flags);

		private static FieldInfo Field(string name, BindingFlags flags)
			=> typeof(ReferenceType).GetField(name, flags);
	}
}
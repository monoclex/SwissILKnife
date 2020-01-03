using System.Reflection;

namespace SwissILKnife.Tests
{
	public partial class ReferenceType
	{
		public static PropertyInfo PropertyInfo_Instance_ReferenceType_Property
			=> Property(nameof(Instance_ReferenceType_Property), BindingFlags.Public | BindingFlags.Instance);

		public string Instance_ReferenceType_Property { get; set; }
	}
}
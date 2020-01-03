using System.Reflection;

namespace SwissILKnife.Tests
{
	public partial class ReferenceType
	{
		public static PropertyInfo PropertyInfo_Static_ReferenceType_Property
			=> Property(nameof(Static_ReferenceType_Property), BindingFlags.Public | BindingFlags.Static);

		public static string Static_ReferenceType_Property { get; set; }
	}
}
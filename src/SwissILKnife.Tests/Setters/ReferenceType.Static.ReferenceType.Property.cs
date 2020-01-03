using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xunit;

namespace SwissILKnife.Tests.Setters
{
	public partial class ReferenceType
	{
		public static PropertyInfo PropertyInfo_Static_ReferenceType_Property
			=> Property(nameof(Static_ReferenceType_Property), BindingFlags.Public | BindingFlags.Static);

		public static string Static_ReferenceType_Property { get; set; }
	}

	public partial class ReferenceTypeTests
	{
		[Fact]
		public void Static_ReferenceType_Property()
		{
			MemberUtils.GenerateSetMethod(ReferenceType.PropertyInfo_Static_ReferenceType_Property)(null, Magic_ReferenceType);
			Assert.Equal(Magic_ReferenceType, ReferenceType.Static_ReferenceType_Property);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xunit;

namespace SwissILKnife.Tests.Setters
{
	public partial class ReferenceType
	{
		public static FieldInfo FieldInfo_Static_ReferenceType_Field
			=> Field(nameof(Static_ReferenceType_Field), BindingFlags.Public | BindingFlags.Static);

		public static string Static_ReferenceType_Field;
	}

	public partial class ReferenceTypeTests
	{
		[Fact]
		public void Static_ReferenceType_Field()
		{
			MemberUtils.GenerateSetMethod(ReferenceType.FieldInfo_Static_ReferenceType_Field)(null, Magic_ReferenceType);
			Assert.Equal(Magic_ReferenceType, ReferenceType.Static_ReferenceType_Field);
		}
	}
}

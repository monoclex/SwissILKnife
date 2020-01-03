using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xunit;

namespace SwissILKnife.Tests.Getters
{
	public partial struct ValueType
	{
		public static FieldInfo FieldInfo_Static_ReferenceType_Field
			=> Field(nameof(Static_ReferenceType_Field), BindingFlags.Public | BindingFlags.Static);

		public static string Static_ReferenceType_Field;
	}

	public partial class ValueTypeTests
	{
		[Fact]
		public void Static_ReferenceType_Field()
		{
			ValueType.Static_ReferenceType_Field = Magic_ReferenceType;

			var getter = MemberUtils.GenerateGetMethod(ValueType.FieldInfo_Static_ReferenceType_Field);
			Assert.Equal(Magic_ReferenceType, getter(null));
		}
	}
}

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xunit;

namespace SwissILKnife.Tests.Getters
{
	public partial struct ValueType
	{
		public static PropertyInfo PropertyInfo_Static_ReferenceType_Property
			=> Property(nameof(Static_ReferenceType_Property), BindingFlags.Public | BindingFlags.Static);

		public static string Static_ReferenceType_Property { get; set; }
	}

	public partial class ValueTypeTests
	{
		[Fact]
		public void Static_ReferenceType_Property()
		{
			ValueType.Static_ReferenceType_Property = Magic_ReferenceType;

			var getter = MemberUtils.GenerateGetMethod(ValueType.PropertyInfo_Static_ReferenceType_Property);
			Assert.Equal(Magic_ReferenceType, getter(null));
		}
	}
}

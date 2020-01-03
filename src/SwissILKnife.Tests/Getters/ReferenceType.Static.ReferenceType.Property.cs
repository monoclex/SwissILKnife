using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xunit;

namespace SwissILKnife.Tests.Getters
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
			ReferenceType.Static_ReferenceType_Property = Magic_ReferenceType;

			var getter = MemberUtils.GenerateGetMethod(ReferenceType.PropertyInfo_Static_ReferenceType_Property);
			Assert.Equal(Magic_ReferenceType, getter(null));
		}
	}
}

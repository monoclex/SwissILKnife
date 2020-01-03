using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xunit;

namespace SwissILKnife.Tests.Setters
{
	public partial class ReferenceType
	{
		public static PropertyInfo PropertyInfo_Instance_ReferenceType_Property
			=> Property(nameof(Instance_ReferenceType_Property), BindingFlags.Public | BindingFlags.Instance);

		public string Instance_ReferenceType_Property { get; set; }
	}

	public partial class ReferenceTypeTests
	{
		[Fact]
		public void Instance_ReferenceType_Property()
		{
			var instance = new ReferenceType();

			MemberUtils.GenerateSetMethod(ReferenceType.PropertyInfo_Instance_ReferenceType_Property)(instance, Magic_ReferenceType);
			Assert.Equal(Magic_ReferenceType, instance.Instance_ReferenceType_Property);
		}
	}
}

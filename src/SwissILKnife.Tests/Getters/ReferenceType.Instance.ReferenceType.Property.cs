using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xunit;

namespace SwissILKnife.Tests.Getters
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
			instance.Instance_ReferenceType_Property = Magic_ReferenceType;

			var getter = MemberUtils.GenerateGetMethod(ReferenceType.PropertyInfo_Instance_ReferenceType_Property);
			Assert.Equal(Magic_ReferenceType, getter(instance));
		}
	}
}

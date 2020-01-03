using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xunit;

namespace SwissILKnife.Tests.Getters
{
	public partial class ReferenceType
	{
		public static FieldInfo FieldInfo_Instance_ReferenceType_Field
			=> Field(nameof(Instance_ReferenceType_Field), BindingFlags.Public | BindingFlags.Instance);

		public string Instance_ReferenceType_Field;
	}

	public partial class ReferenceTypeTests
	{
		[Fact]
		public void Instance_ReferenceType_Field()
		{
			var instance = new ReferenceType();
			instance.Instance_ReferenceType_Field = Magic_ReferenceType;

			var getter = MemberUtils.GenerateGetMethod(ReferenceType.FieldInfo_Instance_ReferenceType_Field);
			Assert.Equal(Magic_ReferenceType, getter(instance));
		}
	}
}

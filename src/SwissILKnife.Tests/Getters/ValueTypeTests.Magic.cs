using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SwissILKnife.Tests.Getters
{
	public partial struct ValueType
	{
		private static PropertyInfo Property(string name, BindingFlags flags)
			=> typeof(ReferenceType).GetProperty(name, flags);

		private static FieldInfo Field(string name, BindingFlags flags)
			=> typeof(ReferenceType).GetField(name, flags);
	}

	public partial class ValueTypeTests
	{
		public const int Magic_ValueType = 0xDEAD;
		public const string Magic_ReferenceType = "Magic String Value";
	}
}

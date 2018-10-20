using System;

namespace SwissILKnife
{
	internal static class Types
	{
		public static readonly Type Void = typeof(void);
		public static readonly Type Object = typeof(object);
		public static readonly Type ObjectArray = typeof(object[]);

		public static readonly Type[] FullyWrappedMethodParameters = new Type[] { Object, ObjectArray };

		public static readonly Type[] OneObjects = new Type[] { Object };
		public static readonly Type[] TwoObjects = new Type[] { Object, Object };
		public static readonly Type[] NoObjects = new Type[] { };
	}
}
namespace SwissILKnife
{
	internal static class Types
	{
		public static readonly Type Void = typeof(void);
		public static readonly Type[] FullyWrappedMethodParameters = new Type[] { TypeOf<object>.Get, TypeOf<object[]>.Get };

		public static readonly Type[] OneObjects = new Type[] { TypeOf<object>.Get };
		public static readonly Type[] TwoObjects = new Type[] { TypeOf<object>.Get, TypeOf<object>.Get };
		public static readonly Type[] NoObjects = new Type[] { };
	}
}
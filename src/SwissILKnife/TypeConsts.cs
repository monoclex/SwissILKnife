using System;

using InstanceInvokable = System.Action<object, object>;
using InvokableReturn = System.Func<object, object>;
using WrappedMethod = System.Func<object, object[], object>;

namespace SwissILKnife
{
	internal static class Types
	{
		public static readonly Type Void = typeof(void);
		public static readonly Type Object = typeof(object);
		public static readonly Type ObjectArray = typeof(object[]);

		public static readonly Type InstanceInvokable = typeof(InstanceInvokable);
		public static readonly Type InvokableReturn = typeof(InvokableReturn);
		public static readonly Type FullyWrappedMethod = typeof(WrappedMethod);
		public static readonly Type[] FullyWrappedMethodParameters = new Type[] { Object, ObjectArray };

		public static readonly Type[] OneObjects = new Type[] { Object };
		public static readonly Type[] TwoObjects = new Type[] { Object, Object };
		public static readonly Type[] NoObjects = new Type[] { };
	}
}
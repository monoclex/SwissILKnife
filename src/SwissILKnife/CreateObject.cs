using StrictEmit;

using System;
using System.Reflection.Emit;

namespace SwissILKnife
{
	public static class InstanceOf<T>
	{
		static InstanceOf()
		{
			var dm = new DynamicMethod(string.Empty, typeof(T), Types.NoObjects, true);
			var il = dm.GetILGenerator();

			il.EmitNewObject<T>();
			il.EmitReturn();

			Constructor = (Func<T>)dm.CreateDelegate(typeof(Func<T>));
		}

		private static readonly Func<T> Constructor;

		public static T Create()
			=> Constructor();
	}

	public static class InstanceOf
	{
		public static Func<object> GetCreator(Type objType)
		{
			var dm = new DynamicMethod(string.Empty, objType, Types.NoObjects, true);
			var il = dm.GetILGenerator();

			il.EmitNewObject(objType.GetConstructor(Type.EmptyTypes));
			il.EmitReturn();

			return (Func<object>)dm.CreateDelegate(typeof(Func<object>));
		}
	}
}
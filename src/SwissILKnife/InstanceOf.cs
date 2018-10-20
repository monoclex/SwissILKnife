using StrictEmit;

using System;

namespace SwissILKnife
{
	public static class InstanceOf<T>
	{
		static InstanceOf()
		{
			var dm = new DynamicMethod<Func<T>>(string.Empty, typeof(T), Types.NoObjects, true)
						.GetILGenerator(out var il);

			il.EmitNewObject<T>();
			il.EmitReturn();

			Constructor = dm.CreateDelegate();
		}

		private static readonly Func<T> Constructor;
		
		public static T Create()
			=> Constructor();
	}
	
	public static class InstanceOf
	{
		
		public static Func<object> GetCreator(Type objType)
		{
			var dm = new DynamicMethod<Func<object>>(string.Empty, objType, Types.NoObjects, true)
						.GetILGenerator(out var il);

			il.EmitNewObject(objType.GetConstructor(Type.EmptyTypes));
			il.EmitReturn();

			return dm.CreateDelegate();
		}
	}
}
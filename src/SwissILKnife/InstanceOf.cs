using MiniStrictEmit;

using System;
using System.Reflection.Emit;

namespace SwissILKnife
{
	/// <summary>
	/// Creates instances of objects using generics.
	/// Generally faster than it's generic-less relative, <see cref="InstanceOf"/>
	/// </summary>
	/// <typeparam name="T">The type of object to be creating</typeparam>
	public static class InstanceOf<T>
	{
		private static readonly Func<T> _constructor;

		static InstanceOf()
		{
			var dm = new DynamicMethod(string.Empty, typeof(T), Types.NoObjects, true)
						.GetILGenerator(out var il);

			il.EmitNewObject<T>();
			il.EmitReturn();

			_constructor = dm.CreateDelegate<Func<T>>();
		}

		/// <summary>
		/// Create an instance of <typeparamref name="T"/>.
		/// See <see cref="InstanceOf"/> if you need to create an instance of an object without generics.
		/// </summary>
		/// <example><code>
		/// public class Test
		/// {
		/// }
		///
		/// Test tst = InstanceOf<Test>.Create();
		/// </code></example>
		/// <typeparam name="T"></typeparam>
		/// <seealso cref="InstanceOf"/>
		public static T Create() => _constructor();
	}

	/// <summary>
	/// Creates instances of objects based on a Type passed in.
	/// Generally slower than it's generic relative, <see cref="InstanceOf{T}"/>
	/// </summary>
	public static class InstanceOf
	{
		/// <summary>
		/// The generic-less version of <see cref="InstanceOf{T}"/>.
		/// Pass in a type to get a <see cref="Func{Object}"/>, which upon called, will return a new object of the desired kind
		/// </summary>
		/// <param name="objType">The type of object to create</param>
		/// <example><code>
		/// public class Test
		/// {
		/// }
		///
		/// Test tst = InstanceOf.GetCreator(typeof(Test))();
		/// </code></example>
		public static Func<object> GetCreator(Type objType)
		{
			var dm = new DynamicMethod(string.Empty, objType, Types.NoObjects, true)
						.GetILGenerator(out var il);

			il.EmitILCreateNewObject(objType);
			il.EmitReturn();

			return dm.CreateDelegate<Func<object>>();
		}

		public static void EmitILCreateNewObject(this ILGenerator il, Type objType)
			=> il.EmitNewObject(objType.GetConstructor(Type.EmptyTypes));
	}
}
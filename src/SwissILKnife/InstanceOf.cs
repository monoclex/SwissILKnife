using MiniStrictEmit;

using System;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace SwissILKnife
{
	/// <summary>
	/// Creates instances of objects using generics.
	/// Generally faster than it's generic-less relative, <see cref="InstanceOf"/>
	/// </summary>
	/// <typeparam name="T">The type of object to be creating</typeparam>
	public static class InstanceOf<T>
	{
		/// <summary>
		/// A method generated with IL that news up an instance of T faster than Activator.CreateInstance.
		/// </summary>
		public static readonly Func<T> New;

		static InstanceOf()
		{
			var dm = new DynamicMethod(string.Empty, typeof(T), Type.EmptyTypes, true)
						.GetILGenerator(out var il);

			il.EmitNewObject<T>();
			il.EmitReturn();

			New = dm.CreateDelegate<Func<T>>();
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
		[Obsolete("Please use 'New'.")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Create() => New();
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
			var dm = new DynamicMethod(string.Empty, objType, Type.EmptyTypes, true)
						.GetILGenerator(out var il);

			il.EmitNewObject(objType.GetConstructor(Type.EmptyTypes));
			il.EmitReturn();

			return dm.CreateDelegate<Func<object>>();
		}
	}
}
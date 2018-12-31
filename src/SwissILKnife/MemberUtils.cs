using MiniStrictEmit;

using System;
using System.Reflection;
using System.Reflection.Emit;

namespace SwissILKnife
{
	public delegate object GetMethod(object instance);
	public delegate void SetMethod(object instance, object value);

	/// <summary>
	/// A class to generate Get and Set methods for getting/setting fields/properties
	/// </summary>
	public static class MemberUtils
	{
		private static void ThrowUnsupportedArgument(MemberInfo member)
			=> throw new ArgumentException($"{nameof(member)} must be a {nameof(PropertyInfo)} or {nameof(FieldInfo)}");

		/// <summary>
		/// Gets a <see cref="GetMethod"/> to get the value of a given <see cref="MemberInfo"/>.
		/// Set the instance to null (first parameter) if it's a static member.
		/// </summary>
		/// <example><code>
		/// public class Test
		/// {
		///		public string Property { get; set; }
		/// }
		///
		/// var tst = new Test();
		/// tst.Property = "5678";
		///
		/// var method = GetGetMethod(typeof(Test).GetProperty(nameof(Test.Property)));
		///
		/// var value = method(tst);
		///
		/// Console.WriteLine(value);
		/// // will output "5678" to the console
		/// </code></example>
		/// <param name="member">The property or field.</param>
		/// <returns>A <see cref="GetMethod"/> that when called upon with the correct parameter, will return the value desired based upon the instance.</returns>
		public static GetMethod GenerateGetMethod(MemberInfo member)
		{
			var dm = new DynamicMethod(string.Empty, TypeOf<object>.Get, Types.OneObjects, member.DeclaringType, true)
						.GetILGenerator(out var il);

			il.EmitGetMethod(member, () => il.EmitLoadArgument(0));
			il.EmitReturn();

			return dm.CreateDelegate<GetMethod>();
		}

		public static void EmitGetMethod(this ILGenerator il, MemberInfo member, Action loadScope)
		{
			if (member is PropertyInfo property)
			{
				il.EmitGetMethod(property, loadScope);
			}
			else if (member is FieldInfo field)
			{
				il.EmitGetMethod(field, loadScope);
			}
			else
			{
				ThrowUnsupportedArgument(member);
			}
		}

		public static void EmitGetMethod(this ILGenerator il, PropertyInfo property, Action loadScope)
		{
			if (!property.SetMethod.IsStatic)
			{
				loadScope();
			}

			il.EmitCallDirect(property.GetMethod);

			if (property.PropertyType.IsValueType)
			{
				il.EmitBox(property.PropertyType);
			}
		}

		public static void EmitGetMethod(this ILGenerator il, FieldInfo field, Action loadScope)
		{
			if (field.IsStatic)
			{
				il.EmitLoadStaticField(field);
			}
			else
			{
				loadScope();

				il.EmitLoadField(field);
			}

			if (field.FieldType.IsValueType)
			{
				il.EmitBox(field.FieldType);
			}
		}

		/// <summary>
		/// Gets a <see cref="SetMethod"/> to set the value of a given <see cref="MemberInfo"/>.
		/// Set the instance to null (first parameter) if it's a static member;
		/// </summary>
		/// <example><code>
		/// public class Test
		/// {
		///		public string Property { get; set; }
		/// }
		///
		/// var tst = new Test();
		///
		/// var method = GetSetMethod(typeof(Test).GetProperty(nameof(Test.Property)));
		///
		/// method(tst, "1234");
		///
		/// Console.WriteLine(tst.Property);
		/// // outputs "1234" to the console
		/// </code></example>
		/// <param name="member">The member.</param>
		/// <returns>A <see cref="SetMethod"/> that when called upon with the correct parameters, update the given <see cref="MemberInfo"/>'s value to the one specified.</returns>
		public static SetMethod GenerateSetMethod(MemberInfo member)
		{
			var dm = new DynamicMethod(string.Empty, Types.Void, Types.TwoObjects, member.DeclaringType, true)
						.GetILGenerator(out var il);

			il.EmitSetMethod(member, () => il.EmitLoadArgument(0), () => il.EmitLoadArgument(1));
			il.EmitReturn();

			return dm.CreateDelegate<SetMethod>();
		}

		public static void EmitSetMethod(this ILGenerator il, MemberInfo member, Action loadScope, Action loadValue)
		{
			if (member is PropertyInfo property)
			{
				il.EmitSetMethod(property, loadScope, loadValue);
			}
			else if (member is FieldInfo field)
			{
				il.EmitSetMethod(field, loadScope, loadValue);
			}
			else
			{
				ThrowUnsupportedArgument(member);
			}
		}

		public static void EmitSetMethod(this ILGenerator il, PropertyInfo property, Action loadScope, Action loadValue)
		{
			if (!property.SetMethod.IsStatic)
			{
				loadScope();
			}

			loadValue();

			if (property.PropertyType.IsValueType)
			{
				il.EmitUnboxAny(property.PropertyType);
			}

			il.EmitCallDirect(property.SetMethod);
		}

		public static void EmitSetMethod(this ILGenerator il, FieldInfo field, Action loadScope, Action loadValue)
		{
			loadScope();

			loadValue();

			if (field.FieldType.IsValueType)
			{
				il.EmitUnboxAny(field.FieldType);
			}

			il.EmitSetField(field);
		}
	}
}
using StrictEmit;
using System;
using System.Reflection;

using InstanceInvokable = System.Action<object, object>;
using InvokableReturn = System.Func<object, object>;

namespace SwissILKnife
{
	/// <summary>
	/// A class to generate Get and Set methods for getting/setting fields/properties
	/// </summary>
	public static class MemberUtils
	{
		private static void ThrowUnsupportedArgument(MemberInfo member)
			=> throw new UnsupportedArgumentTypeException(
					$"{nameof(member)}'s type isn't supported. Please ensure that {nameof(member)} is either a {nameof(PropertyInfo)}, or a {nameof(FieldInfo)}");

		/// <summary>
		/// Gets a <see cref="InstanceInvokable"/> to set the value of a given <see cref="MemberInfo"/>.
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
		/// <returns>A <see cref="InstanceInvokable"/> that when called upon with the correct parameters, update the given <see cref="MemberInfo"/>'s value to the one specified.</returns>
		public static InstanceInvokable GetSetMethod(MemberInfo member)
		{
			var dm = new DynamicMethod<InstanceInvokable>(string.Empty, Types.Void, Types.TwoObjects, member.DeclaringType, true)
						.GetILGenerator(out var il);

			if (member is PropertyInfo property)
			{
				if (!property.SetMethod.IsStatic)
				{
					il.EmitLoadArgument(0);
				}

				il.EmitLoadArgument(1);

                if (property.PropertyType.IsValueType)
                {
                    il.EmitUnboxAny(property.PropertyType);
                }

                il.EmitCallDirect(property.SetMethod);
			}
			else if (member is FieldInfo field)
			{
				il.EmitLoadArgument(0);
                
				il.EmitLoadArgument(1);

                il.EmitUnboxAny(field.FieldType);

                il.EmitSetField(field);
			}
			else
			{
				ThrowUnsupportedArgument(member);
			}

			il.EmitReturn();

			return dm.CreateDelegate();
		}

		/// <summary>
		/// Gets a <see cref="InvokableReturn"/> to get the value of a given <see cref="MemberInfo"/>.
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
		/// <returns>A <see cref="InvokableReturn"/> that when called upon with the correct parameter, will return the value desired based upon the instance.</returns>
		public static InvokableReturn GetGetMethod(MemberInfo member)
		{
			var dm = new DynamicMethod<InvokableReturn>(string.Empty, TypeOf<object>.Get, Types.OneObjects, member.DeclaringType, true)
						.GetILGenerator(out var il);

			if (member is PropertyInfo property)
			{
				if (!property.SetMethod.IsStatic)
				{
					il.EmitLoadArgument(0);
				}

				il.EmitCallDirect(property.GetMethod);

                if (property.PropertyType.IsValueType)
                {
                    il.EmitBox(property.PropertyType);
                }
			}
			else if (member is FieldInfo field)
			{
				if (field.IsStatic)
				{
					il.EmitLoadStaticField(field);
				}
				else
				{
					il.EmitLoadArgument(0);

					il.EmitLoadField(field);
				}
                
                if (field.FieldType.IsValueType)
                {
                    il.EmitBox(field.FieldType);
                }
            }
			else
			{
				ThrowUnsupportedArgument(member);
			}

			il.EmitReturn();

			return dm.CreateDelegate();
		}
	}
}
using StrictEmit;

using System.Reflection;
using System.Reflection.Emit;

using InstanceInvokable = System.Action<object, object>;
using InvokableReturn = System.Func<object, object>;

namespace SwissILKnife
{
	public static class MemberUtils
	{
		private static void ThrowUnsupportedArgument(MemberInfo member)
			=> throw new UnsupportedArgumentTypeException(
					$"{nameof(member)}'s type isn't supported. Please ensure that {nameof(member)} is either a {nameof(PropertyInfo)}, or a {nameof(FieldInfo)}");

		public static InstanceInvokable GetSetMethod(MemberInfo member)
		{
			var dm = new DynamicMethod(string.Empty, Types.Void, Types.TwoObjects, member.DeclaringType, true);
			var il = dm.GetILGenerator();

			if (member is PropertyInfo property)
			{
				if (!property.SetMethod.IsStatic)
				{
					il.EmitLoadArgument(0);
				}

				il.EmitLoadArgument(1);

				il.EmitCallDirect(property.SetMethod);
			}
			else if (member is FieldInfo field)
			{
				il.EmitLoadArgument(0);
				il.EmitLoadArgument(1);

				il.EmitSetField(field);
			}
			else
			{
				ThrowUnsupportedArgument(member);
			}

			il.EmitReturn();

			return (InstanceInvokable)dm.CreateDelegate(Types.InstanceInvokable);
		}

		public static InvokableReturn GetGetMethod(MemberInfo member)
		{
			var dm = new DynamicMethod(string.Empty, Types.Object, Types.OneObjects, member.DeclaringType, true);
			var il = dm.GetILGenerator();

			if (member is PropertyInfo property)
			{
				if (!property.SetMethod.IsStatic)
				{
					il.EmitLoadArgument(0);
				}

				il.EmitCallDirect(property.GetMethod);
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
			}
			else
			{
				ThrowUnsupportedArgument(member);
			}

			il.EmitReturn();

			return (InvokableReturn)dm.CreateDelegate(Types.InvokableReturn);
		}
	}
}
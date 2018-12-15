using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace MiniStrictEmit
{
	public static class MiniStrictEmitExtensions
	{
		public static void EmitUnboxAny(this ILGenerator il, Type type)
		{
		}

		public static void EmitSetLocalVariable(this ILGenerator il, LocalBuilder local)
		{
		}

		public static void EmitSetField(this ILGenerator il, FieldInfo field)
		{
		}

		public static void EmitSetArrayElement(this ILGenerator il, Type arrayType)
		{
		}

		public static void EmitReturn(this ILGenerator il)
		{
		}

		public static void EmitNewObject<T>(this ILGenerator il)
		{
		}

		public static void EmitNewObject(this ILGenerator il, ConstructorInfo ctor)
		{
		}

		public static void EmitLoadArgument(this ILGenerator il, int arg)
		{
		}

		public static void EmitCallDirect(this ILGenerator il, MethodInfo method)
		{
		}

		public static void EmitBox(this ILGenerator il, Type type)
		{
		}

		public static void EmitLoadStaticField(this ILGenerator il, FieldInfo field)
		{
		}

		public static void EmitLoadField(this ILGenerator il, FieldInfo field)
		{
		}

		public static void EmitConstantInt(this ILGenerator il, int val)
		{
		}

		public static void EmitLoadArrayElement(this ILGenerator il, Type type)
		{
		}

		public static void EmitLoadLocalVariableAddress(this ILGenerator il, LocalBuilder local)
		{
		}

		public static void EmitCallVirtual(this ILGenerator il, MethodInfo method)
		{
		}

		public static void EmitLoadLocalVariable(this ILGenerator il, LocalBuilder local)
		{
		}

		public static void EmitLoadNull(this ILGenerator il)
		{
		}
	}
}

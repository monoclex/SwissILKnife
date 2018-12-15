

using SwissILKnife;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace StrictEmit
{
	public static class MiniStrictEmitExtensions
	{
		public static void EmitSetArrayElement(this ILGenerator il, Type t)
			=> il.Emit(OpCodes.Stelem, t);

		public static void EmitLoadArrayElement(this ILGenerator il, Type t)
			=> il.Emit(OpCodes.Ldelem, t);

		public static void EmitCallDirect(this ILGenerator il, MethodInfo method)
			=> il.Emit(OpCodes.Call, method);

		public static void EmitCallVirtual(this ILGenerator il, MethodInfo method)
			=> il.Emit(OpCodes.Callvirt, method);

		public static void EmitNewObject<T>(this ILGenerator il)
			=> il.EmitNewObject(typeof(T).GetConstructor(new Type[] { }));

		public static void EmitNewObject(this ILGenerator il, ConstructorInfo ctor)
			=> il.Emit(OpCodes.Newobj, ctor);

		public static void EmitBox(this ILGenerator il, Type type)
			=> il.Emit(OpCodes.Box, type);

		public static void EmitUnboxAny(this ILGenerator il, Type type)
			=> il.Emit(OpCodes.Unbox_Any, type);

		public static void EmitLoadNull(this ILGenerator il)
			=> il.Emit(OpCodes.Ldnull);

		public static void EmitSetField(this ILGenerator il, FieldInfo field)
			=> il.Emit(OpCodes.Stfld, field);

		public static void EmitReturn(this ILGenerator il)
			=> il.Emit(OpCodes.Ret);

		public static void EmitLoadStaticField(this ILGenerator il, FieldInfo field)
			=> il.Emit(OpCodes.Ldsfld, field);

		public static void EmitLoadField(this ILGenerator il, FieldInfo field)
			=> il.Emit(OpCodes.Ldfld, field);

		public static void EmitConstantInt(this ILGenerator il, int value)
		{
			switch (value)
			{
				case -1: il.Emit(OpCodes.Ldc_I4_M1); return;
				case 0: il.Emit(OpCodes.Ldc_I4_0); return;
				case 1: il.Emit(OpCodes.Ldc_I4_1); return;
				case 2: il.Emit(OpCodes.Ldc_I4_2); return;
				case 3: il.Emit(OpCodes.Ldc_I4_3); return;
				case 4: il.Emit(OpCodes.Ldc_I4_4); return;
				case 5: il.Emit(OpCodes.Ldc_I4_5); return;
				case 6: il.Emit(OpCodes.Ldc_I4_6); return;
				case 7: il.Emit(OpCodes.Ldc_I4_7); return;
				case 8: il.Emit(OpCodes.Ldc_I4_8); return;

				default:
				{

					if (value > 8 && value <= 255)
					{
						il.Emit(OpCodes.Ldc_I4_S, (byte)value);
					}
					else
					{
						il.Emit(OpCodes.Ldc_I4, value);
					}
					return;
				}
			}
		}

		// variables
		
		public static void EmitSetLocalVariable(this ILGenerator il, LocalBuilder local)
		{
			var indx = local.LocalIndex;

			switch(indx)
			{
				case 0: il.Emit(OpCodes.Stloc_0); return;
				case 1: il.Emit(OpCodes.Stloc_1); return;
				case 2: il.Emit(OpCodes.Stloc_2); return;
				case 3: il.Emit(OpCodes.Stloc_3); return;

				default:
				{
					if (indx > 3 && indx <= 255)
					{
						il.Emit(OpCodes.Stloc_S, (byte)indx);
					}
					else
					{
						il.Emit(OpCodes.Stloc, indx);
					}
					return;
				}
			}
		}

		public static void EmitLoadArgument(this ILGenerator il, int arg)
		{
			switch (arg)
			{
				case 0: il.Emit(OpCodes.Ldarg_0); return;
				case 1: il.Emit(OpCodes.Ldarg_1); return;
				case 2: il.Emit(OpCodes.Ldarg_2); return;
				case 3: il.Emit(OpCodes.Ldarg_3); return;

				default:
				{
					if (arg > 3 && arg <= 255)
					{
						il.Emit(OpCodes.Ldarg_S, (byte)arg);
					}
					else
					{
						il.Emit(OpCodes.Ldarg, arg);
					}
					return;
				}
			}
		}

		public static void EmitLoadLocalVariable(this ILGenerator il, LocalBuilder local)
		{
			var indx = local.LocalIndex;

			switch (indx)
			{
				case 0: il.Emit(OpCodes.Ldloc_0); return;
				case 1: il.Emit(OpCodes.Ldloc_1); return;
				case 2: il.Emit(OpCodes.Ldloc_2); return;
				case 3: il.Emit(OpCodes.Ldloc_3); return;

				default:
				{
					if (indx > 3 && indx <= 255)
					{
						il.Emit(OpCodes.Ldloc_S, (byte)indx);
					}
					else
					{
						il.Emit(OpCodes.Ldloc, indx);
					}
					return;
				}
			}
		}

		public static void EmitLoadLocalVariableAddress(this ILGenerator il, LocalBuilder local)
		{
			var indx = local.LocalIndex;

			if (indx >= 0 && indx <= 255)
			{
				il.Emit(OpCodes.Ldloca_S, (byte)indx);
			}
			else
			{
				il.Emit(OpCodes.Ldloca, indx);
			}
		}
	}
}

/**/
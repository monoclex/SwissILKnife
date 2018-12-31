using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace MiniStrictEmit
{
	internal static class MiniStrictEmitExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void EmitSetArrayElement(this ILGenerator il, Type t)
			=> il.Emit(OpCodes.Stelem, t);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void EmitLoadArrayElement(this ILGenerator il, Type t)
			=> il.Emit(OpCodes.Ldelem, t);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void EmitCallDirect(this ILGenerator il, MethodInfo method)
			=> il.Emit(OpCodes.Call, method);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void EmitCallVirtual(this ILGenerator il, MethodInfo method)
			=> il.Emit(OpCodes.Callvirt, method);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void EmitNewObject<T>(this ILGenerator il)
			=> il.EmitNewObject(typeof(T).GetConstructor(new Type[] { }));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void EmitNewObject(this ILGenerator il, ConstructorInfo ctor)
			=> il.Emit(OpCodes.Newobj, ctor);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void EmitBox(this ILGenerator il, Type type)
			=> il.Emit(OpCodes.Box, type);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void EmitUnboxAny(this ILGenerator il, Type type)
			=> il.Emit(OpCodes.Unbox_Any, type);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void EmitLoadNull(this ILGenerator il)
			=> il.Emit(OpCodes.Ldnull);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void EmitSetField(this ILGenerator il, FieldInfo field)
			=> il.Emit(OpCodes.Stfld, field);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void EmitReturn(this ILGenerator il)
			=> il.Emit(OpCodes.Ret);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void EmitLoadStaticField(this ILGenerator il, FieldInfo field)
			=> il.Emit(OpCodes.Ldsfld, field);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void EmitLoadField(this ILGenerator il, FieldInfo field)
			=> il.Emit(OpCodes.Ldfld, field);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void EmitConstantInt(this ILGenerator il, int value)
		{
			switch (value)
			{
				case -1: il.Emit(OpCodes.Ldc_I4_M1); break;
				case 0: il.Emit(OpCodes.Ldc_I4_0); break;
				case 1: il.Emit(OpCodes.Ldc_I4_1); break;
				case 2: il.Emit(OpCodes.Ldc_I4_2); break;
				case 3: il.Emit(OpCodes.Ldc_I4_3); break;
				case 4: il.Emit(OpCodes.Ldc_I4_4); break;
				case 5: il.Emit(OpCodes.Ldc_I4_5); break;
				case 6: il.Emit(OpCodes.Ldc_I4_6); break;
				case 7: il.Emit(OpCodes.Ldc_I4_7); break;
				case 8: il.Emit(OpCodes.Ldc_I4_8); break;

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
					break;
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void EmitSetLocalVariable(this ILGenerator il, LocalBuilder local)
		{
			var indx = local.LocalIndex;

			switch (indx)
			{
				case 0: il.Emit(OpCodes.Stloc_0); break;
				case 1: il.Emit(OpCodes.Stloc_1); break;
				case 2: il.Emit(OpCodes.Stloc_2); break;
				case 3: il.Emit(OpCodes.Stloc_3); break;

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
					break;
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void EmitLoadArgument(this ILGenerator il, int arg)
		{
			switch (arg)
			{
				case 0: il.Emit(OpCodes.Ldarg_0); break;
				case 1: il.Emit(OpCodes.Ldarg_1); break;
				case 2: il.Emit(OpCodes.Ldarg_2); break;
				case 3: il.Emit(OpCodes.Ldarg_3); break;

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
					break;
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void EmitLoadLocalVariable(this ILGenerator il, LocalBuilder local)
		{
			var indx = local.LocalIndex;

			switch (indx)
			{
				case 0: il.Emit(OpCodes.Ldloc_0); break;
				case 1: il.Emit(OpCodes.Ldloc_1); break;
				case 2: il.Emit(OpCodes.Ldloc_2); break;
				case 3: il.Emit(OpCodes.Ldloc_3); break;

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
					break;
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
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
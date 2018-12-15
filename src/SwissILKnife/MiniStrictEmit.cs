using SwissILKnife;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace MiniStrictEmit
{
	public static class MiniStrictEmitExtensions
	{
		// arrays

		public static void EmitStoreArrayElementObject(this ILGenerator il)
		{
			il.Emit(OpCodes.Stelem, TypeOf<object>.Get);
		}

		public static void EmitLoadArrayElementObject(this ILGenerator il)
		{
			il.Emit(OpCodes.Ldelem, TypeOf<object>.Get);
		}

		// calls

		public static void EmitCallDirect(this ILGenerator il, MethodInfo method)
		{
			il.Emit(OpCodes.Call, method);
		}

		public static void EmitCallVirtual(this ILGenerator il, MethodInfo method)
		{
			il.Emit(OpCodes.Callvirt, method);
		}

		// objects

		public static void EmitNewObject<T>(this ILGenerator il)
		{
			il.EmitNewObject(typeof(T).GetConstructor(new Type[] { }));
		}

		public static void EmitNewObject(this ILGenerator il, ConstructorInfo ctor)
		{
			il.Emit(OpCodes.Newobj, ctor);
		}

		// values

		public static void EmitBox(this ILGenerator il, Type type)
		{
			il.Emit(OpCodes.Box, type);
		}

		public static void EmitUnboxAny(this ILGenerator il, Type type)
		{
			il.Emit(OpCodes.Unbox_Any, type);
		}

		private static readonly OpCode[] EmitConstantIntOps = new OpCode[]
		{
			OpCodes.Ldc_I4_M1,
			OpCodes.Ldc_I4_0,
			OpCodes.Ldc_I4_1,
			OpCodes.Ldc_I4_2,
			OpCodes.Ldc_I4_3,
			OpCodes.Ldc_I4_4,
			OpCodes.Ldc_I4_5,
			OpCodes.Ldc_I4_6,
			OpCodes.Ldc_I4_7,
			OpCodes.Ldc_I4_8
		};

		public static void EmitConstantInt(this ILGenerator il, int value)
		{
			var arrIndx = value + 1;

			if (arrIndx >= 0 && arrIndx <= EmitConstantIntOps.Length)
			{
				il.Emit(EmitConstantIntOps[arrIndx]);
				return;
			}

			if (value > EmitConstantIntOps.Length && value <= 255)
			{
				il.Emit(OpCodes.Ldc_I4_S, (byte)value);
			}
			else
			{
				il.Emit(OpCodes.Ldc_I4, value);
			}
		}

		public static void EmitLoadNull(this ILGenerator il)
		{
			il.Emit(OpCodes.Ldnull);
		}

		// variables

		private static readonly OpCode[] EmitSetLocalVariableConsts = new OpCode[]
		{
			OpCodes.Stloc_0,
			OpCodes.Stloc_1,
			OpCodes.Stloc_2,
			OpCodes.Stloc_3,
		};

		public static void EmitSetLocalVariable(this ILGenerator il, LocalBuilder local)
		{
			var indx = local.LocalIndex;

			if (indx >= 0 && indx < EmitSetLocalVariableConsts.Length)
			{
				il.Emit(EmitSetLocalVariableConsts[indx]);
				return;
			}

			if (indx >= EmitSetLocalVariableConsts.Length && indx <= 255)
			{
				il.Emit(OpCodes.Stloc_S, (byte)indx);
			}
			else
			{
				il.Emit(OpCodes.Stloc, indx);
			}
		}

		public static void EmitSetField(this ILGenerator il, FieldInfo field)
		{
			il.Emit(OpCodes.Stfld, field);
		}

		public static void EmitReturn(this ILGenerator il)
		{
			il.Emit(OpCodes.Ret);
		}

		private static readonly OpCode[] EmitLoadArgumentConsts = new OpCode[]
		{
			OpCodes.Ldarg_0,
			OpCodes.Ldarg_1,
			OpCodes.Ldarg_2,
			OpCodes.Ldarg_3,
		};

		public static void EmitLoadArgument(this ILGenerator il, int arg)
		{
			if (arg >= 0 && arg < EmitLoadArgumentConsts.Length)
			{
				il.Emit(EmitLoadArgumentConsts[arg]);
				return;
			}

			if (arg >= EmitConstantIntOps.Length && arg <= 255)
			{
				il.Emit(OpCodes.Ldarg_S, (byte)arg);
			}
			else
			{
				il.Emit(OpCodes.Ldarg, (short)arg);
			}
		}

		public static void EmitLoadStaticField(this ILGenerator il, FieldInfo field)
		{
			il.Emit(OpCodes.Ldsfld, field);
		}

		public static void EmitLoadField(this ILGenerator il, FieldInfo field)
		{
			il.Emit(OpCodes.Ldfld, field);
		}

		public static void EmitLoadLocalVariableAddress(this ILGenerator il, LocalBuilder local)
		{
			var indx = local.LocalIndex;

			if (indx >= 4 && indx <= 255)
			{
				il.Emit(OpCodes.Ldloca_S, (byte)indx);
			}
			else
			{
				il.Emit(OpCodes.Ldloca, indx);
			}
		}

		private static readonly OpCode[] EmitLoadLocalVariableConsts = new OpCode[]
		{
			OpCodes.Ldloc_0,
			OpCodes.Ldloc_1,
			OpCodes.Ldloc_2,
			OpCodes.Ldloc_3,
		};

		public static void EmitLoadLocalVariable(this ILGenerator il, LocalBuilder local)
		{
			var indx = local.LocalIndex;

			if (indx >= 0 && indx < EmitLoadLocalVariableConsts.Length)
			{
				il.Emit(EmitLoadLocalVariableConsts[indx]);
				return;
			}

			if (indx >= EmitLoadLocalVariableConsts.Length && indx <= 255)
			{
				il.Emit(OpCodes.Ldloc_S, (byte)indx);
			}
			else
			{
				il.Emit(OpCodes.Ldloc, indx);
			}
		}
	}
}

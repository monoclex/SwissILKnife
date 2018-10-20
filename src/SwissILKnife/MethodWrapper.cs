#if DEBUG && (NET45 || NET472)
#define DISKSAVING
#endif

using StrictEmit;

using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using FullyWrappedMethod = System.Func<object, object[], object>;

namespace SwissILKnife
{
	public static class MethodWrapper
	{
		public static FullyWrappedMethod Wrap(MethodInfo method
#if DISKSAVING
			, bool saveToDisk = false
#endif
			)
		{
#if DISKSAVING
			var asm = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("TestAssembly"), AssemblyBuilderAccess.RunAndSave);
			var mod = asm.DefineDynamicModule("TestModule", "asm.dll", true);
			var cls = mod.DefineType("SomeClass", TypeAttributes.Public | TypeAttributes.Class);
			var dm2 = cls.DefineMethod("Test", MethodAttributes.Public, Types.Object, Types.FullyWrappedMethodParameters);

			var il2 = dm2.GetILGenerator();

			EmitWrapIL(il2, method);
#endif

			var dm = new DynamicMethod<FullyWrappedMethod>(string.Empty, Types.Object, Types.FullyWrappedMethodParameters, method.DeclaringType, true)
				.GetILGenerator(out var il);

			EmitWrapIL(il, method);

#if DISKSAVING
			if (saveToDisk)
			{
				cls.CreateType();

				asm.Save("asm.dll", PortableExecutableKinds.ILOnly, ImageFileMachine.AMD64);
			}
#endif

			return dm.CreateDelegate();
		}

		private static void EmitWrapIL(ILGenerator il, MethodInfo method)
		{
			var parameters = method.GetParameters();

			const int _zero = 0;
			const int _one = 1;

			var localNumber = 0;

			if (!method.IsStatic)
			{
				il.EmitLoadArgument(_zero);
				il.EmitUnboxAny(method.DeclaringType);
			}

			for (var i = 0; i < parameters.Length; i++)
			{
				var param = parameters[i];

				if (!param.IsOut)
				{
					il.EmitLoadArgument(_one);
					il.EmitConstantInt(i);
					il.EmitLoadArrayElement(Types.Object);
				}

				if (param.IsValueType())
				{
					il.EmitUnboxAny(param.ParameterType);
				}
			}

			var locals = parameters
							.Where(x => x.IsNeedsSetting())
							.Select(x => {
								var local = il.DeclareLocal(x.ParameterType.GetElementType());
								il.EmitLoadLocalVariableAddress(local);
								return local;
							})
							.ToArray();

			if (method.IsStatic)
			{
				il.EmitCallDirect(method);
			}
			else
			{
				il.EmitCallVirtual(method);
			}

			if (locals.Length > 0)
			{
				localNumber = 0;

				for (var i = 0; i < parameters.Length; i++) //TODO: be areful about ++idx
				{
					var param = parameters[i];

					if (param.IsNeedsSetting())
					{
						var local = locals[localNumber];

						il.EmitLoadArgument(_one);
						il.EmitConstantInt(i);
						il.EmitLoadLocalVariable(local);

						var elemType = param.ParameterType.GetElementType();

						if (elemType.IsValueType)
						{
							il.EmitBox(elemType);
						}

						il.EmitSetArrayElement(Types.Object);

						localNumber++;
					}
				}
			}

			if (method.ReturnType == null ||
				method.ReturnType == Types.Void)
			{
				il.EmitLoadNull();
			}
			else if (method.ReturnType.IsValueType)
			{
				il.EmitBox(method.ReturnType);
			}

			il.EmitReturn();
		}
	}
}
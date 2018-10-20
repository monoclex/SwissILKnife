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
	/// <summary>
	/// Wraps entire <see cref="MethodInfo"/>s into a <see cref="System.Delegate"/>
	/// </summary>
	public static class MethodWrapper
	{
		/// <summary>Wraps the specified method.</summary>
		/// <example><code>
		/// public class Test
		/// {
		///		public bool Method(string a, out int b)
		///			=> int.Tryparse(a, out b);
		/// }
		///
		/// var wrapped = Wrap(typeof(Test).GetMethod(nameof(Test.Method)));
		///
		/// var tst = new Test();
		/// var args = new object[] { "1234", null };
		/// var result = (bool)wrapped(tst, args);
		/// var parsedInt = (int)args[1];
		/// </code></example>
		/// <param name="method">The method to wrap</param>
		/// <returns>A <see cref="FullyWrappedMethod"/> that acts similar to invoking the method</returns>
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
			var dm2 = cls.DefineMethod("Test", MethodAttributes.Public, TypeOf<object>.Type, Types.FullyWrappedMethodParameters);

			var il2 = dm2.GetILGenerator();

			EmitWrapIL(il2, method);
#endif

			var dm = new DynamicMethod<FullyWrappedMethod>(string.Empty, TypeOf<object>.Get, Types.FullyWrappedMethodParameters, method.DeclaringType, true)
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
					il.EmitLoadArrayElement(TypeOf<object>.Get);
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

						il.EmitSetArrayElement(TypeOf<object>.Get);

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
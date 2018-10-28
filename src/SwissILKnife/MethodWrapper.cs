#if (NET45 || NET472) && DEBUG
#define DISKSAVING
#endif

using StrictEmit;

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
		public static FullyWrappedMethod Wrap(MethodInfo method)
		{
			var dm = new DynamicMethod<FullyWrappedMethod>(string.Empty, TypeOf<object>.Get, Types.FullyWrappedMethodParameters, method.DeclaringType, true)
				.GetILGenerator(out var il);

			EmitWrapIL(il, method);

			return dm.CreateDelegate();
		}

#if DISKSAVING
		public static void SaveWrap(MethodInfo method, string dllName)
		{
			var asm = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("TestAssembly"), AssemblyBuilderAccess.RunAndSave);
			var mod = asm.DefineDynamicModule("TestModule", "asm.dll", true);
			var cls = mod.DefineType("SomeClass", TypeAttributes.Public | TypeAttributes.Class);
			var dm = cls.DefineMethod("Test", MethodAttributes.Public, TypeOf<object>.Get, Types.FullyWrappedMethodParameters);
			var il = dm.GetILGenerator();

			EmitWrapIL(il, method);

			cls.CreateType();
			asm.Save(dllName, PortableExecutableKinds.ILOnly, ImageFileMachine.AMD64);
		}
#endif

		private static void EmitWrapIL(ILGenerator il, MethodInfo method)
		{
			var parameters = method.GetParameters();

			const int indexZero = 0;
			const int indexOne = 1;

			if (!method.IsStatic)
			{
				il.EmitLoadArgument(indexZero);
				il.EmitUnboxAny(method.DeclaringType);
			}

			for (var i = 0; i < parameters.Length; i++)
			{
				var param = parameters[i];

				if (!param.IsOutOrRef())
				{
					il.EmitLoadArgument(indexOne);
					il.EmitConstantInt(i);
					il.EmitLoadArrayElement(TypeOf<object>.Get);
				}

				if (param.IsValueType())
				{
					il.EmitUnboxAny(param.ParameterType);
				}
			}

			var locals = new LocalBuilder[parameters.Length];

			for (var i = 0; i < locals.Length; i++)
			{
				var parameter = parameters[i];

				if (parameter.IsOutOrRef())
				{
					var local = il.DeclareLocal(parameter.ParameterType.GetElementType());

					if (parameter.IsByRef())
					{
						il.EmitLoadArgument(indexOne);
						il.EmitConstantInt(i);
						il.EmitLoadArrayElement(TypeOf<object>.Get);

						if (parameter.IsValueType())
						{
							il.EmitUnboxAny(parameter.ParameterType);
						}

						il.EmitSetLocalVariable(local);
					}

					il.EmitLoadLocalVariableAddress(local);

					locals[i] = local;
				}
			}

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
				for (var i = 0; i < parameters.Length; i++) //TODO: be areful about ++idx
				{
					var param = parameters[i];

					if (param.IsOutOrRef())
					{
						var local = locals[i];

						il.EmitLoadArgument(indexOne);
						il.EmitConstantInt(i);
						il.EmitLoadLocalVariable(local);

						var elemType = param.ParameterType.GetElementType();

						if (elemType.IsValueType)
						{
							il.EmitBox(elemType);
						}

						il.EmitSetArrayElement(TypeOf<object>.Get);
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
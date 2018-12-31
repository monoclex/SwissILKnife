#if (NET45 || NET472) && DEBUG
#define DISKSAVING
#endif

using MiniStrictEmit;

using System;
using System.Reflection;
using System.Reflection.Emit;

namespace SwissILKnife
{
	public delegate object Wrapped(object instance, object[] parameters);

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
		/// <returns>A <see cref="Wrapped"/> that acts similar to invoking the method</returns>
		public static Wrapped Wrap(MethodInfo method)
		{
			var dm = new DynamicMethod(string.Empty, TypeOf<object>.Get, Types.FullyWrappedMethodParameters, method.DeclaringType, true)
				.GetILGenerator(out var il);

			il.EmitILWrap(method, () => il.EmitLoadArgument(0), () => il.EmitLoadArgument(1));
			il.EmitReturn();

			return dm.CreateDelegate<Wrapped>();
		}

		public static void EmitILWrap(this ILGenerator il, MethodInfo method, Action loadScope, Action loadObjectArray)
		{
			var parameters = method.GetParameters();

			if (!method.IsStatic)
			{
				loadScope();
				il.EmitUnboxAny(method.DeclaringType);
			}

			for (var i = 0; i < parameters.Length; i++)
			{
				var param = parameters[i];

				if (!param.IsOutOrRef())
				{
					loadObjectArray();
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
						loadObjectArray();
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
				// TODO: be careful about ++i
				for (var i = 0; i < parameters.Length; i++)
				{
					var param = parameters[i];

					if (param.IsOutOrRef())
					{
						var local = locals[i];

						loadObjectArray();
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
		}
	}
}
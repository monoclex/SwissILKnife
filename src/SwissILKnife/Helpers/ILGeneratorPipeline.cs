using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace SwissILKnife
{
	public class ILGeneratorPipeline : AbstractILGenerator.AbstractILGenerator
	{
		private readonly ILGenerator _main;
		private readonly ILGenerator[] _chain;

		private readonly ILGenerator[] _all;

		public ILGeneratorPipeline(ILGenerator main, params ILGenerator[] chain)
		{
			_main = main ?? throw new ArgumentNullException(nameof(main));
			_chain = chain ?? throw new ArgumentNullException(nameof(chain));

			_all = new ILGenerator[1 + _chain.Length];

			_all[0] = _main;
			Array.Copy(_chain, 0, _all, 1, _chain.Length);
		}

		//
		// the idea is to cache the result of every single ILGenerator, and then find the right cached one
		// then we'll pass the correct one to every ILGenerator
		//
		// seemless integration :D
		//

		// todo: refactor? this is duplicated code
		private List<Label[]> _labels = new List<Label[]>();
		private List<LocalBuilder[]> _locals = new List<LocalBuilder[]>();

		private Label[] FindLabels(Label mainLabel) => _labels.Find((array) => array[0].Equals(mainLabel));
		private LocalBuilder[] FindLocals(LocalBuilder mainLocal) => _locals.Find((array) => array[0].Equals(mainLocal));

		private Label Cache(Label[] lables)
		{
			_labels.Add(lables);
			return lables[0];
		}

		private LocalBuilder Cache(LocalBuilder[] locals)
		{
			_locals.Add(locals);
			return locals[0];
		}

		// everything that returns an object we have to cache

		public override Label BeginExceptionBlock() => Cache(_all.All((ilgen) => ilgen.BeginExceptionBlock()));
		public override LocalBuilder DeclareLocal(Type localType) => Cache(_all.All((ilgen) => ilgen.DeclareLocal(localType)));
		public override LocalBuilder DeclareLocal(Type localType, bool pinned) => Cache(_all.All((ilgen) => ilgen.DeclareLocal(localType, pinned)));
		public override Label DefineLabel() => Cache(_all.All((ilgen) => ilgen.DefineLabel()));

		public override int ILOffset => _main.ILOffset;

		public override void BeginCatchBlock(Type exceptionType) => _all.All((ilgen) => ilgen.BeginCatchBlock(exceptionType));
		public override void BeginExceptFilterBlock() => _all.All((ilgen) => ilgen.BeginExceptionBlock());
		public override void BeginFaultBlock() => _all.All((ilgen) => ilgen.BeginFaultBlock());
		public override void BeginFinallyBlock() => _all.All((ilgen) => ilgen.BeginFinallyBlock());
		public override void BeginScope() => _all.All((ilgen) => ilgen.BeginScope());
		public override void Emit(OpCode opcode) => _all.All((ilgen) => ilgen.Emit(opcode));
		public override void Emit(OpCode opcode, byte arg) => _all.All((ilgen) => ilgen.Emit(opcode, arg));
		public override void Emit(OpCode opcode, double arg) => _all.All((ilgen) => ilgen.Emit(opcode, arg));
		public override void Emit(OpCode opcode, float arg) => _all.All((ilgen) => ilgen.Emit(opcode, arg));
		public override void Emit(OpCode opcode, int arg) => _all.All((ilgen) => ilgen.Emit(opcode, arg));
		public override void Emit(OpCode opcode, Label label) => _all.All(FindLabels(label), (ilgen, i, emitlabel) => ilgen.Emit(opcode, emitlabel));

		public override void Emit(OpCode opcode, Label[] labels)
		{
			var forEveryLabel = new List<Label[]>();

			foreach(var i in labels)
			{
				forEveryLabel.Add(FindLabels(i));
			}

			_all.All(forEveryLabel, (ilgen, i, emitlabels) => ilgen.Emit(opcode, emitlabels));
		}

		public override void Emit(OpCode opcode, LocalBuilder local) => _all.All(FindLocals(local), (ilgen, i, emitlocal) => ilgen.Emit(opcode, emitlocal));
		public override void Emit(OpCode opcode, long arg) => _all.All((ilgen) => ilgen.Emit(opcode, arg));
		public override void Emit(OpCode opcode, short arg) => _all.All((ilgen) => ilgen.Emit(opcode, arg));
		public override void Emit(OpCode opcode, SignatureHelper signature) => _all.All((ilgen) => ilgen.Emit(opcode, signature));
		public override void Emit(OpCode opcode, string str) => _all.All((ilgen) => ilgen.Emit(opcode, str));
		public override void Emit(OpCode opcode, ConstructorInfo con) => _all.All((ilgen) => ilgen.Emit(opcode, con));
		public override void Emit(OpCode opcode, FieldInfo field) => _all.All((ilgen) => ilgen.Emit(opcode, field));
		public override void Emit(OpCode opcode, MethodInfo meth) => _all.All((ilgen) => ilgen.Emit(opcode, meth));
		public override void Emit(OpCode opcode, Type cls) => _all.All((ilgen) => ilgen.Emit(opcode, cls));
		public override void EmitCall(OpCode opcode, MethodInfo methodInfo, Type[] optionalParameterTypes) => _all.All((ilgen) => ilgen.EmitCall(opcode, methodInfo, optionalParameterTypes));
		public override void EmitCalli(OpCode opcode, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, Type[] optionalParameterTypes) => _all.All((ilgen) => ilgen.EmitCalli(opcode, callingConvention, returnType, parameterTypes, optionalParameterTypes));
		public override void EmitWriteLine(FieldInfo fld) => _all.All((ilgen) => ilgen.EmitWriteLine(fld));
		public override void EmitWriteLine(LocalBuilder localBuilder) => _all.All((ilgen) => ilgen.EmitWriteLine(localBuilder));
		public override void EmitWriteLine(string value) => _all.All((ilgen) => ilgen.EmitWriteLine(value));
		public override void EndExceptionBlock() => _all.All((ilgen) => ilgen.EndExceptionBlock());
		public override void EndScope() => _all.All((ilgen) => ilgen.EndScope());
		public override void MarkLabel(Label loc) => _all.All(FindLabels(loc), (ilgen, i, emitlabel) => ilgen.MarkLabel(emitlabel));
		public override void ThrowException(Type excType) => _all.All((ilgen) => ilgen.ThrowException(excType));
		public override void UsingNamespace(string usingNamespace) => _all.All((ilgen) => ilgen.UsingNamespace(usingNamespace));
	}

	internal static class Extension
	{
		public static void All(this ILGenerator[] ilgenerators, Action<ILGenerator> ilgenAction)
		{
			foreach (var generator in ilgenerators)
			{
				ilgenAction(generator);
			}
		}

		public static void PassAll<T>(this ILGenerator[] ilgenerators, Action<ILGenerator, int> ilgenAction)
		{
			for (int i = 0; i < ilgenerators.Length; i++)
			{
				ilgenAction(ilgenerators[i], i);
			}
		}

		public static void All<T>(this ILGenerator[] ilgenerators, IEnumerable<T> itemsEnumerable, Action<ILGenerator, int, T> ilgenAction)
		{
			T[] items = itemsEnumerable.ToArray();
			for (int i = 0; i < ilgenerators.Length; i++)
			{
				ilgenAction(ilgenerators[i], i, items[i]);
			}
		}

		public static T[] All<T>(this ILGenerator[] ilgenerators, Func<ILGenerator, T> ilgenFunc)
		{
			T[] result = new T[ilgenerators.Length];

			for(var i = 0; i < ilgenerators.Length; i++)
			{
				result[i] = ilgenFunc(ilgenerators[i]);
			}

			return result;
		}

		/*
		 * a more generic version of this function:
		 *
		public static void All<T>(this IEnumerable<T> items, Action<T> itemAction)
		{
			foreach(var item in items)
			{
				itemAction(item);
			}
		}
		*/
	}
}

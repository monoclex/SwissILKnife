using System;
using System.Reflection;
using System.Reflection.Emit;
using SwissILKnife;

/*
 *			Purpose of this project:
 *
 * This allows us to save the IL we generate from the MethodWrapper.Wrap function.
 * In the future, the saving feature will be extended, and perhaps even opened to the public.
 *
 * For now, this is a "development-only" thing, to help spot bugs in IL.
 *
 */

namespace SwissILKnife.ILDebugging
{
	public class Wrong
	{
		public string Test { get; set; }
		public override int GetHashCode() => 5;
	}

	internal class Program
	{
		private static void Main()
		{
			object myInt = 123;
			object myString = "456";

			var asm = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("TestAssembly"), AssemblyBuilderAccess.RunAndSave);
			var mod = asm.DefineDynamicModule("TestModule", "asm.dll", true);
			var cls = mod.DefineType("SomeClass", TypeAttributes.Public | TypeAttributes.Class);
			var dm = cls.DefineMethod("Test", MethodAttributes.Public, typeof(object), new Type[] { typeof(object), typeof(object[]) });
			var il = dm.GetILGenerator();



			cls.CreateType();
			asm.Save("asm.dll", PortableExecutableKinds.ILOnly, ImageFileMachine.AMD64);

			Console.ReadLine();
		}
	}
}
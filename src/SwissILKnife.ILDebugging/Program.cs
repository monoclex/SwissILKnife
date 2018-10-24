using System;
using System.Reflection;
using System.Runtime.InteropServices;
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
		public override int GetHashCode() => 5;
	}

		internal class Program
	{
		public readonly MethodInfo OutAndRefFunc =
			typeof(Program).GetMethod(nameof(OutAndRefWrap));

		public bool OutAndRefWrap(int a, ref string b, out int c)
		{
			var integer = int.Parse(b);
			b = integer.ToString() + " = INT";

			c = a - integer;

			return c > 0;
		}

		public void WrapsOutFunc()
		{
			var args = new object[] { 5, "123", null };

			MethodWrapper.SaveWrap(OutAndRefFunc, "asm.dll");
		}

		private static void Main()
		{
			Console.WriteLine(typeof(uint).GetHashCode());
			Console.WriteLine(typeof(Wrong).GetHashCode());

			var typeCache = new TypeCache<string>();

			typeCache[typeof(uint)] = "1234";
			Console.WriteLine(typeCache[typeof(uint)]);

			typeCache[typeof(Wrong)] = "5678";
			Console.WriteLine(typeCache[typeof(uint)]);
			Console.WriteLine(typeCache[typeof(Wrong)]);

			Console.ReadLine();

			new Program().WrapsOutFunc();
			Console.WriteLine("Saved");
			Console.ReadLine();
		}
	}
}
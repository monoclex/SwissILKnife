using System;
using System.Reflection;

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

			// MethodWrapper.SaveWrap(OutAndRefFunc, "asm.dll");
		}

		private static void Main()
		{
			object myInt = 123;
			object myString = "456";

			Console.WriteLine(Cast1(myInt));
			Console.WriteLine(Cast2(myString));

			Console.ReadLine();
		}

		public static object Cast1(object input)
		{
			return (int)input;
		}

		public static object Cast2(object input)
		{
			return (string)input;
		}
	}
}
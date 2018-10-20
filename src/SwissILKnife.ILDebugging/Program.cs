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
	internal class Program
	{
		public readonly MethodInfo OutFuncExample =
			typeof(Program).GetMethod(nameof(OutFuncWrap));

		public bool OutFuncWrap(string input, out int result) => int.TryParse(input, out result);

		public void WrapsOutFunc()
		{
			var args = new object[] { "1234", null };
		}

		private static void Main()
		{
			new Program().WrapsOutFunc();
			Console.WriteLine("Saved");
			Console.ReadLine();
		}
	}
}
using System;

/*
 *
 * TODO: clearly label and name each benchmark
 *
 * We need consistent naming on the stuff, and
 * by the time I'm done typing it, I could've
 * already done it.
 *
 */

namespace SwissILKnife.Benchmarks
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			BenchmarkDotNet.Running.BenchmarkRunner.Run<SetComparisons>();
			Console.ReadLine();
		}
	}
}
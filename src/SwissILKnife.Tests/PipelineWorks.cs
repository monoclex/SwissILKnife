using FluentAssertions;

using System;
using System.Reflection.Emit;

using Xunit;

namespace SwissILKnife.Tests
{
	public class PipelineWorks
	{
		[Fact]
		public void Test()
		{
			var dyn1 = new DynamicMethod(string.Empty, typeof(string), new Type[] { });
			var dyn2 = new DynamicMethod(string.Empty, typeof(string), new Type[] { });

			var pipeline = new ILGeneratorPipeline(dyn1.GetILGenerator(), dyn2.GetILGenerator());

			pipeline.Emit(OpCodes.Ldstr, "test");
			pipeline.Emit(OpCodes.Ret);

			(dyn1.CreateDelegate<Func<string>>()).Invoke()
				.Should()
				.Be((dyn2.CreateDelegate<Func<string>>()).Invoke());
		}
	}
}
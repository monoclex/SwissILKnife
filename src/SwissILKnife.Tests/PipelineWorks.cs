using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using Xunit;
using FluentAssertions;

namespace SwissILKnife.Tests
{
	public class PipelineWorks
	{
		[Fact]
		public void Test()
		{
			var dyn1 = new DynamicMethod<Func<string>>(string.Empty, typeof(string), new Type[] { });
			var dyn2 = new DynamicMethod<Func<string>>(string.Empty, typeof(string), new Type[] { });

			var pipeline = new ILGeneratorPipeline(dyn1.ILGenerator, dyn2.ILGenerator);

			pipeline.Emit(OpCodes.Ldstr, "test");
			pipeline.Emit(OpCodes.Ret);

			(dyn1.CreateDelegate())()
				.Should()
				.Be((dyn2.CreateDelegate())());
		}
	}
}

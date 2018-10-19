using Xunit;

namespace SwissILKnife.Tests
{
	public class SampleClass
	{
		public SampleClass() => Test = "test";

		public string Test { get; set; }
	}

	public class CreatesInstances
	{
		[Fact]
		public void InstanceOfGeneric()
		{
			var t = InstanceOf<SampleClass>.Create();

			Assert.Equal("test", t.Test);
		}

		[Fact]
		public void InstanceOfObject()
		{
			var t = (SampleClass)(InstanceOf.GetCreator(typeof(SampleClass))());

			Assert.Equal("test", t.Test);
		}
	}
}
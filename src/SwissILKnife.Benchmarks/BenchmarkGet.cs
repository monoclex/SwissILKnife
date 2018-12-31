using BenchmarkDotNet.Attributes;

using System;
using System.Reflection;

namespace SwissILKnife.Benchmarks
{
	public class BenchmarkGet
	{
		public string SomeProperty { get; set; }
		public string SomeField;

		private readonly PropertyInfo _property;
		private readonly FieldInfo _field;

		private readonly GetMethod _getProperty;
		private readonly GetMethod _getField;

		public BenchmarkGet()
		{
			_property = typeof(BenchmarkGet).GetProperty(nameof(SomeProperty), BindingFlags.Public | BindingFlags.Instance);
			_field = typeof(BenchmarkGet).GetField(nameof(SomeField), BindingFlags.Public | BindingFlags.Instance);

			_getProperty = MemberUtils.GenerateGetMethod(_property);
			_getField = MemberUtils.GenerateGetMethod(_field);
		}

		[Benchmark]
		public string GetFieldNormally()
			=> SomeField;

		[Benchmark]
		public string GetPropertyNormally()
			=> SomeProperty;

		[Benchmark]
		public GetMethod GenerateGetFieldViaIL()
			=> MemberUtils.GenerateGetMethod(_field);

		[Benchmark]
		public GetMethod GenerateGetPropertyViaIL()
			=> MemberUtils.GenerateGetMethod(_property);

		[Benchmark]
		public object GetFieldViaIL()
			=> _getField(this);

		[Benchmark]
		public object GetPropertyViaIL()
			=> _getProperty(this);
	}
}
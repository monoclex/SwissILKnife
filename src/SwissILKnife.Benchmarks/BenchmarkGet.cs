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

		private readonly Func<object, object> _getProperty;
		private readonly Func<object, object> _getField;

		public BenchmarkGet()
		{
			_property = typeof(BenchmarkGet).GetProperty(nameof(SomeProperty), BindingFlags.Public | BindingFlags.Instance);
			_field = typeof(BenchmarkGet).GetField(nameof(SomeField), BindingFlags.Public | BindingFlags.Instance);

			_getProperty = MemberUtils.GetGetMethod(_property);
			_getField = MemberUtils.GetGetMethod(_field);
		}

		[Benchmark]
		public string GetFieldNormally()
			=> SomeField;

		[Benchmark]
		public string GetPropertyNormally()
			=> SomeProperty;

		[Benchmark]
		public Func<object, object> GenerateGetFieldViaIL()
			=> MemberUtils.GetGetMethod(_field);

		[Benchmark]
		public Func<object, object> GenerateGetPropertyViaIL()
			=> MemberUtils.GetGetMethod(_property);

		[Benchmark]
		public object GetFieldViaIL()
			=> _getField(this);

		[Benchmark]
		public object GetPropertyViaIL()
			=> _getProperty(this);
	}
}
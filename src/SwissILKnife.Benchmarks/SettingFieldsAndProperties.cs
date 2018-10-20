using BenchmarkDotNet.Attributes;

using System.Reflection;

namespace SwissILKnife.Benchmarks
{
	public class SettingFieldsAndProperties
	{
		public int SomeProperty { get; set; }
		public int SomeField;

		private PropertyInfo _property = typeof(SettingFieldsAndProperties).GetProperty(nameof(SomeProperty));
		private FieldInfo _field = typeof(SettingFieldsAndProperties).GetField(nameof(SomeField));

		[Params(1, 21, 41)]
		public int Reps;

		public SettingFieldsAndProperties()
		{
		}

		[Benchmark]
		public void ReflectionSetProperty()
		{
			for (var i = 0; i < Reps; i++)
				_property.SetValue(this, 3);
		}

		[Benchmark]
		public void ReflectionSetField()
		{
			for (var i = 0; i < Reps; i++)
				_field.SetValue(this, 3);
		}

		[Benchmark]
		public void ReflectionGetProperty()
		{
			for (var i = Reps - 1; i >= 0; i--)
				_property.GetValue(this);
		}

		[Benchmark]
		public void ReflectionGetField()
		{
			for (var i = 0; i < Reps; i++)
				_field.GetValue(this);
		}

		[Benchmark]
		public void SwissILSetProperty()
		{
			var swiss = MemberUtils.GetSetMethod(_property);
			for (var i = 0; i < Reps; i++)
				swiss(this, 3);
		}

		[Benchmark]
		public void SwissILSetField()
		{
			var swiss = MemberUtils.GetSetMethod(_field);
			for (var i = 0; i < Reps; i++)
				swiss(this, 3);
		}

		[Benchmark]
		public void SwissILGetProperty()
		{
			var swiss = MemberUtils.GetGetMethod(_property);
			for (var i = 0; i < Reps; i++)
				swiss(this);
		}

		[Benchmark]
		public void SwissILGetField()
		{
			var swiss = MemberUtils.GetGetMethod(_field);
			for (var i = 0; i < Reps; i++)
				swiss(this);
		}
	}
}
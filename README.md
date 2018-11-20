
# SwissILKnife

![bannerad]

Swiss IL Knife is a "Swiss Army Knife" of sorts for IL-generated methods that are faster then their Expression & Reflection counterparts.

```
PM> Package-Install SwissILKnife -pre
```

## Library very WIP

Have an idea for something that can be IL Generated, or a suggestion? Submit an issue!

Currently, SwissILKnife can wrap `MethodInfo`s into `Func<object, object[], object>`s, and create getters/setters for `PropertyInfo`.

## Example

```cs
public class SomeClass
{
    public string MyProperty { get; set; }
}

static class Program
{
    private static Action<object, object> MyPropertySetter;
    private static PropertyInfo myProperty = typeof(SomeClass)
                                                 .GetProperty(nameof(SomeClass.MyProperty));

    static Program()
    {
        MyPropertySetter = SwissILKnife.MemberUtils.GetSetMethod(myProperty);
    }

    static void Main(string[] args)
    {
        var sClass = new SomeClass();

        // represented as PropReflectionSet in the benchmarks below
        myProperty.SetValue(sClass, "1234");
        PrintValueOfMyProperty(sClass); // "1234"

        // represented as PropInvokeSetViaSwissIL in the benchmarks below
        MyPropertySetter(sClass, "5678");
        PrintValueOfMyProperty(sClass); // "5678"
    }

    private static void PrintValueOfMyProperty(SomeClass sClass)
        => Console.WriteLine($"\"{sClass.MyProperty}\"");
}
```

*Note: the following benchmark does not account for the time required to generate the SwissIL invoker*
```
                      Method |           Mean |         Error |        StdDev |
---------------------------- |---------------:|--------------:|--------------:|
           PropReflectionSet |     193.071 ns |     2.7146 ns |     2.5393 ns |
     PropInvokeSetViaSwissIL |       3.320 ns |     0.0622 ns |     0.0582 ns |
```

As for invoker generation time, SwissIL is *leagues* ahead of Expressions.

*Note: the following benchmark contains the time required in order to generate the Swiss IL invoker. (`6,792.274 ns`)*
```
                      Method |           Mean |         Error |        StdDev |
---------------------------- |---------------:|--------------:|--------------:|
 PropCreateSetViaExpressions | 157,808.724 ns | 2,251.9861 ns | 1,880.5102 ns |
     PropCreateSetViaSwissIL |   6,792.274 ns |   131.3222 ns |   188.3384 ns |
```

[bannerad]: ./banner_ad.png
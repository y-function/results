extern alias ResultClass;
extern alias ResultClassExtensions;
extern alias ResultStruct;
extern alias ResultStructExtensions;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using ResultC = ResultClass::Y.Results.Result;
using ResultClassExtensions::Y.Results;
using ResultS = ResultStruct::Y.Results.Result;
using ResultStructExtensions::Y.Results;

namespace Benchmark;

public class ThenInOutClasses : Benchmark
{
    [Benchmark(Baseline = true, Description = "Plain · sync then (classes)", OperationsPerInvoke = OperationsCount)]
    public long Then_Plain()
    {
        long sum = 0;
        for (int i = 0; i < OperationsCount; i++)
        {
            try
            {
                var v = Plain_Step3(Plain_Step2(Plain_Step1(Plain_Step0())));
                sum += v;
            }
            catch (Exception)
            {
                sum += 1;
            }
        }

        return sum;
    }

    [Benchmark(Description = "Class · sync then (classes)", OperationsPerInvoke = OperationsCount)]
    public long Then_Class()
    {
        long sum = 0;
        for (int i = 0; i < OperationsCount; i++)
        {
            var r = ResultC.Success(new TestClass { TestProp = 42 })
                .Then(x => new TestClass2 { TestProp = x.TestProp + 1 })
                .Then(x => new TestClass3 { TestProp = x.TestProp + 2 })
                .Then(x => x.TestProp);
            sum += r.Value;
        }

        return sum;
    }

    [Benchmark(Description = "Struct · sync then (classes)", OperationsPerInvoke = OperationsCount)]
    public long Then_Struct()
    {
        long sum = 0;
        for (int i = 0; i < OperationsCount; i++)
        {
            var r = ResultS.Success(new TestClass { TestProp = 42 })
                .Then(x => new TestClass2 { TestProp = x.TestProp + 1 })
                .Then(x => new TestClass3 { TestProp = x.TestProp + 2 })
                .Then(x => x.TestProp);
            sum += r.Value;
        }

        return sum;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static TestClass Plain_Step0() => new() { TestProp = 42 };

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static TestClass2 Plain_Step1(TestClass x) => new() { TestProp = x.TestProp + 1 };

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static TestClass3 Plain_Step2(TestClass2 x) => new() { TestProp = x.TestProp + 2 };

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int Plain_Step3(TestClass3 x) => x.TestProp;
}

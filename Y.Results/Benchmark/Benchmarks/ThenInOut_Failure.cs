extern alias ResultClass;
extern alias ResultClassExtensions;
extern alias ResultStruct;
extern alias ResultStructExtensions;
using BenchmarkDotNet.Attributes;
using ResultC = ResultClass::Y.Results.Result;
using ResultClassExtensions::Y.Results;
using ResultS = ResultStruct::Y.Results.Result;
using ResultStructExtensions::Y.Results;

namespace Benchmark;

public class ThenInOutFailure : Benchmark
{
    [Benchmark(Baseline = true, Description = "Plain · sync then failure (throw + try/catch)", OperationsPerInvoke = OperationsCount)]
    public long ThenFailure_Plain()
    {
        long sum = 0;
        for (int i = 0; i < OperationsCount; i++)
        {
            try
            {
                sum += PlainSteps.Step_Throwing(i);
            }
            catch (InvalidOperationException)
            {
                // equivalent to a failed Result: no value, the error is handled at the boundary
            }
        }

        return sum;
    }

    [Benchmark(Description = "Class · sync then failure (int)", OperationsPerInvoke = OperationsCount)]
    public long ThenFailure_Class()
    {
        long sum = 0;
        for (int i = 0; i < OperationsCount; i++)
        {
            var r = ResultC.Failure<int>(new Exception())
                .Then(x => x + 1)
                .Then(x => x + 2)
                .Then(x => x + 3);
            sum += r.Value;
        }

        return sum;
    }

    [Benchmark(Description = "Struct · sync then failure (int)", OperationsPerInvoke = OperationsCount)]
    public long ThenFailure_Struct()
    {
        long sum = 0;
        for (int i = 0; i < OperationsCount; i++)
        {
            var r = ResultS.Failure<int>(new Exception())
                .Then(x => x + 1)
                .Then(x => x + 2)
                .Then(x => x + 3);
            sum += r.Value;
        }

        return sum;
    }
}

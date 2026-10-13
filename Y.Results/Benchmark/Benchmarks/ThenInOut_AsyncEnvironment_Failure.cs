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

public class ThenInOutAsyncEnvironmentFailure : Benchmark
{
    [Benchmark(Baseline = true, Description = "Plain · async pipeline failure (throw + try/catch)",
        OperationsPerInvoke = OperationsCount)]
    public async Task<int> PlainFailureBench()
    {
        var sum = 0;
        for (var i = 0; i < OperationsCount; i++)
        {
            var result = await GetEndpointPlainFailureAsync(i);
            sum += result;
        }

        return sum;
    }

    [Benchmark(Description = "Class · async pipeline failure", OperationsPerInvoke = OperationsCount)]
    public async Task<int> ResultClassFailureBench()
    {
        var sum = 0;
        for (var i = 0; i < OperationsCount; i++)
        {
            var result = await GetEndpointResultClassFailureAsync(i);
            sum += result.Value;
        }

        return sum;
    }

    [Benchmark(Description = "Struct · async pipeline failure", OperationsPerInvoke = OperationsCount)]
    public async Task<int> ResultStructFailureBench()
    {
        var sum = 0;
        for (var i = 0; i < OperationsCount; i++)
        {
            var result = await GetEndpointResultStructFailureAsync(i);
            sum += result.Value;
        }

        return sum;
    }

    private static async Task<int> GetEndpointPlainFailureAsync(int i)
    {
        try
        {
            var task = await GetServicePlainFailureAsync(i);
            var task2 = await GetServicePlainFailureAsync2(task);
            return task2.TestProp;
        }
        catch (InvalidOperationException)
        {
            // equivalent to a failed Result: no value, the error is handled at the boundary
            return 0;
        }
    }

    private static async Task<TestClass3> GetServicePlainFailureAsync(int i)
    {
        try
        {
            var repo = await Repo_PlainFailureAsync(i);
            var s1 = new TestClass { TestProp = repo.TestProp };
            var s2 = new TestClass2 { TestProp = s1.TestProp + 1 };
            return new TestClass3 { TestProp = s2.TestProp + 2 };
        }
        catch (Exception e)
        {
            throw;
        }
    }

    private static async Task<TestClass3> GetServicePlainFailureAsync2(TestClass3 testClass)
    {
        var repo = await Repo_PlainFailureAsync(testClass.TestProp);
        var s1 = new TestClass { TestProp = testClass.TestProp + repo.TestProp };
        var s2 = new TestClass2 { TestProp = s1.TestProp + 1 };
        return new TestClass3 { TestProp = s2.TestProp + 2 };
    }

    private static Task<TestClass> Repo_PlainFailureAsync(int i)
        => throw new InvalidOperationException($"Repo failed for iteration {i}.");

    private static async Task<ResultClass::Y.Results.Result<int>> GetEndpointResultClassFailureAsync(int i)
    {
        var task = await GetServiceResultClassFailureAsync(i);
        var task2 = await task.Then(GetServiceResultClassFailureAsync2);
        var result = task2.Then(x => x.TestProp);

        return result;
    }

    private static async Task<ResultClass::Y.Results.Result<TestClass3>> GetServiceResultClassFailureAsync(int i)
    {
        var repo = await Repo_ClassFailureAsync(i);
        var s1 = repo.Then(x => new TestClass { TestProp = x.TestProp });
        var s2 = s1.Then(x => new TestClass2 { TestProp = x.TestProp + 1 });
        return s2.Then(x => new TestClass3 { TestProp = x.TestProp + 2 });
    }

    private static async Task<ResultClass::Y.Results.Result<TestClass3>> GetServiceResultClassFailureAsync2(
        TestClass3 testClass)
    {
        var repo = await Repo_ClassFailureAsync(testClass.TestProp);
        var s1 = repo.Then(x => new TestClass { TestProp = x.TestProp });
        var s2 = s1.Then(x => new TestClass2 { TestProp = x.TestProp + 1 });
        return s2.Then(x => new TestClass3 { TestProp = x.TestProp + 2 });
    }

    private static Task<ResultClass::Y.Results.Result<TestClass>> Repo_ClassFailureAsync(int i)
        => Task.FromResult(
            ResultC.Failure<TestClass>(new InvalidOperationException($"Repo failed for iteration {i}.")));

    private static async Task<ResultStruct::Y.Results.Result<int>> GetEndpointResultStructFailureAsync(int i)
    {
        var task = await GetServiceResultStructFailureAsync(i);
        var task2 = await task.Then(GetServiceResultStructFailureAsync2);
        var result = task2.Then(x => x.TestProp);

        return result;
    }

    private static async Task<ResultStruct::Y.Results.Result<TestClass3>> GetServiceResultStructFailureAsync(int i)
    {
        var repo = await Repo_StructFailureAsync(i);
        var s1 = repo.Then(x => new TestClass { TestProp = x.TestProp });
        var s2 = s1.Then(x => new TestClass2 { TestProp = x.TestProp + 1 });
        return s2.Then(x => new TestClass3 { TestProp = x.TestProp + 2 });
    }

    private static async Task<ResultStruct::Y.Results.Result<TestClass3>> GetServiceResultStructFailureAsync2(
        TestClass3 testClass)
    {
        var repo = await Repo_StructFailureAsync(testClass.TestProp);
        var s1 = repo.Then(x => new TestClass { TestProp = x.TestProp });
        var s2 = s1.Then(x => new TestClass2 { TestProp = x.TestProp + 1 });
        return s2.Then(x => new TestClass3 { TestProp = x.TestProp + 2 });
    }

    private static Task<ResultStruct::Y.Results.Result<TestClass>> Repo_StructFailureAsync(int i)
        => Task.FromResult(
            ResultS.Failure<TestClass>(new InvalidOperationException($"Repo failed for iteration {i}.")));
}
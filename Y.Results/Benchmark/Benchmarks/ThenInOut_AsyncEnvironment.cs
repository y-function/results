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

public class ThenInOutAsyncEnvironment : Benchmark
{
    [Benchmark(Baseline = true, Description = "Class · async pipeline")]
    public async Task<int> ResultClassBench()
    {
        var sum = 0;
        for (var i = 0; i < OperationsCount; i++)
        {
            var result = await GetEndpointResultClassAsync();
            sum += result.Value;
        }

        return sum;
    }

    [Benchmark(Description = "Struct · async pipeline")]
    public async Task<int> ResultStructBench()
    {
        var sum = 0;
        for (var i = 0; i < OperationsCount; i++)
        {
            var result = await GetEndpointResultStructAsync();
            sum += result.Value;
        }

        return sum;
    }

    private async Task<ResultClass::Y.Results.Result<int>> GetEndpointResultClassAsync()
    {
        var task = await GetServiceResultClassAsync();
        var result = task.Then(x => x.TestProp);

        return result;
    }

    private async Task<ResultClass::Y.Results.Result<TestClass3>> GetServiceResultClassAsync()
    {
        var repo = await Repo_ClassAsync();
        var s1 = repo.Then(x => new TestClass { TestProp = x.TestProp });
        var s2 = s1.Then(x => new TestClass2 { TestProp = x.TestProp + 1 });
        return s2.Then(x => new TestClass3 { TestProp = x.TestProp + 2 });
    }

    private static Task<ResultClass::Y.Results.Result<TestClass>> Repo_ClassAsync()
        => Task.FromResult(ResultC.Success(new TestClass { TestProp = 42 }));

    private async Task<ResultStruct::Y.Results.Result<int>> GetEndpointResultStructAsync()
    {
        var task = await GetServiceResultStructAsync();
        var result = task.Then(x => x.TestProp);

        return result;
    }

    private async Task<ResultStruct::Y.Results.Result<TestClass3>> GetServiceResultStructAsync()
    {
        var repo = await Repo_StructAsync();
        var s1 = repo.Then(x => new TestClass { TestProp = x.TestProp });
        var s2 = s1.Then(x => new TestClass2 { TestProp = x.TestProp + 1 });
        return s2.Then(x => new TestClass3 { TestProp = x.TestProp + 2 });
    }

    private static Task<ResultStruct::Y.Results.Result<TestClass>> Repo_StructAsync()
        => Task.FromResult(ResultS.Success(new TestClass { TestProp = 42 }));
}

public class TestClass
{
    public int TestProp { get; set; }
}

public class TestClass2
{
    public int TestProp { get; set; }
}

public class TestClass3
{
    public int TestProp { get; set; }
}
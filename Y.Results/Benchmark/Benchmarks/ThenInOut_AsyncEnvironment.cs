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
    [Benchmark(Baseline = true, Description = "Plain · async pipeline (no result)",
        OperationsPerInvoke = OperationsCount)]
    public async Task<int> PlainBench()
    {
        var sum = 0;
        for (var i = 0; i < OperationsCount; i++)
        {
            var result = await GetEndpointPlainAsync(i);
            sum += result;
        }

        return sum;
    }

    [Benchmark(Description = "Class · async pipeline", OperationsPerInvoke = OperationsCount)]
    public async Task<int> ResultClassBench()
    {
        var sum = 0;
        for (var i = 0; i < OperationsCount; i++)
        {
            var result = await GetEndpointResultClassAsync(i);
            sum += result.Value;
        }

        return sum;
    }

    [Benchmark(Description = "Struct · async pipeline", OperationsPerInvoke = OperationsCount)]
    public async Task<int> ResultStructBench()
    {
        var sum = 0;
        for (var i = 0; i < OperationsCount; i++)
        {
            var result = await GetEndpointResultStructAsync(i);
            sum += result.Value;
        }

        return sum;
    }

    private static async Task<int> GetEndpointPlainAsync(int i)
    {
        try
        {
            var task = await GetServicePlainAsync(i);
            var task2 = await GetServicePlainAsync2(task);
            return task2.TestProp;
        }
        catch (Exception)
        {
            return 0;
        }
    }

    private static async Task<TestClass3> GetServicePlainAsync(int i)
    {
        try
        {
            var repo = await Repo_PlainAsync(i);
            var s1 = new TestClass { TestProp = repo.TestProp };
            var s2 = new TestClass2 { TestProp = s1.TestProp + 1 };
            return new TestClass3 { TestProp = s2.TestProp + 2 };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private static async Task<TestClass3> GetServicePlainAsync2(TestClass3 testClass)
    {
        var repo = await Repo_PlainAsync(testClass.TestProp);
        var s1 = new TestClass { TestProp = testClass.TestProp + repo.TestProp };
        var s2 = new TestClass2 { TestProp = s1.TestProp + 1 };
        return new TestClass3 { TestProp = s2.TestProp + 2 };
    }

    private static Task<TestClass> Repo_PlainAsync(int i)
        => Task.FromResult(new TestClass { TestProp = i });

    private async Task<ResultClass::Y.Results.Result<int>> GetEndpointResultClassAsync(int i)
    {
        var task = await GetServiceResultClassAsync(i);
        var task2 = await task.Then(GetServiceResultClassAsync2);
        var result = task2.Then(x => x.TestProp);

        return result;
    }

    private async Task<ResultClass::Y.Results.Result<TestClass3>> GetServiceResultClassAsync(int i)
    {
        var repo = await Repo_ClassAsync(i);
        var s1 = repo.Then(x => new TestClass { TestProp = x.TestProp });
        var s2 = s1.Then(x => new TestClass2 { TestProp = x.TestProp + 1 });
        return s2.Then(x => new TestClass3 { TestProp = x.TestProp + 2 });
    }

    private async Task<ResultClass::Y.Results.Result<TestClass3>> GetServiceResultClassAsync2(TestClass3 testClass)
    {
        var repo = await Repo_ClassAsync(testClass.TestProp);
        var s1 = repo.Then(x => new TestClass { TestProp = testClass.TestProp + x.TestProp });
        var s2 = s1.Then(x => new TestClass2 { TestProp = x.TestProp + 1 });
        return s2.Then(x => new TestClass3 { TestProp = x.TestProp + 2 });
    }

    private static Task<ResultClass::Y.Results.Result<TestClass>> Repo_ClassAsync(int i)
        => Task.FromResult(ResultC.Success(new TestClass { TestProp = i }));

    private async Task<ResultStruct::Y.Results.Result<int>> GetEndpointResultStructAsync(int i)
    {
        var task = await GetServiceResultStructAsync(i);
        var task2 = await task.Then(GetServiceResultStructAsync2);
        var result = task2.Then(x => x.TestProp);

        return result;
    }

    private async Task<ResultStruct::Y.Results.Result<TestClass3>> GetServiceResultStructAsync(int i)
    {
        var repo = await Repo_StructAsync(i);
        var s1 = repo.Then(x => new TestClass { TestProp = x.TestProp });
        var s2 = s1.Then(x => new TestClass2 { TestProp = x.TestProp + 1 });
        return s2.Then(x => new TestClass3 { TestProp = x.TestProp + 2 });
    }

    private async Task<ResultStruct::Y.Results.Result<TestClass3>> GetServiceResultStructAsync2(TestClass3 testClass)
    {
        var repo = await Repo_StructAsync(testClass.TestProp);
        var s1 = repo.Then(x => new TestClass { TestProp = testClass.TestProp + x.TestProp });
        var s2 = s1.Then(x => new TestClass2 { TestProp = x.TestProp + 1 });
        return s2.Then(x => new TestClass3 { TestProp = x.TestProp + 2 });
    }

    private static Task<ResultStruct::Y.Results.Result<TestClass>> Repo_StructAsync(int i)
        => Task.FromResult(ResultS.Success(new TestClass { TestProp = i }));
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
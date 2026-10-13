using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Benchmark;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net80, launchCount: 1, warmupCount: 10, iterationCount: 40)]
// [SimpleJob(RuntimeMoniker.Net10_0, launchCount: 1, warmupCount: 10, iterationCount: 10)]
// [SimpleJob(RuntimeMoniker.Net80)]
// [SimpleJob(RuntimeMoniker.Net10_0)]
public abstract class Benchmark
{
    protected const int OperationsCount = 5_000;
}
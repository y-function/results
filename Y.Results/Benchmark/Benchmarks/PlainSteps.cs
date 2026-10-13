using System.Runtime.CompilerServices;

namespace Benchmark;

/// <summary>
/// Plain C# building blocks (no Results) shared by the "Plain" benchmarks.
/// NoInlining keeps the three "repo -> service -> endpoint" layers as real calls,
/// so the plain pipeline is comparable to the Result pipelines instead of being fully elided.
/// </summary>
internal static class PlainSteps
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static int Step1(int x) => x + 1;

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static int Step2(int x) => x + 2;

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static int Step3(int x) => x + 3;

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static int Step_Throwing(int x) => throw new InvalidOperationException($"Step 1 failed for input {x}.");
}

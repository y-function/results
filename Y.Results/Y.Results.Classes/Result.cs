using System.Runtime.CompilerServices;

namespace Y.Results;

#if CLASSES
public partial class Result
#elif STRUCTS
public readonly partial struct Result
#endif
{
    public static bool TryGetFailure(out Result failure, params ReadOnlySpan<Result> results)
    {
        foreach (ref readonly var result in results)
        {
            if (result.IsSuccess) continue;

            failure = result;
            return true;
        }

        failure = default!;
        return false;
    }

    public static bool TryGetFailure<T>(out Result<T> failure, params ReadOnlySpan<Result<T>> results)
    {
        foreach (ref readonly var result in results)
        {
            if (result.IsSuccess) continue;

            failure = result;
            return true;
        }

        failure = default!;
        return false;
    }

    public Exception? Exception { get; }
    public bool IsSuccess { get; }

    internal Result(bool isSuccess, Exception? exception = null)
    {
        IsSuccess = isSuccess;
        Exception = exception;
    }
}

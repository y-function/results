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
            AssertNotDefault(result);
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

    internal static void AssertNotDefault(Result result, [CallerArgumentExpression(nameof(result))] string? paramName = null)
    {
#if CLASSES
        if (result == null)
#elif STRUCTS
        if (result == default)
#endif
            throw new ArgumentNullException(paramName);
    }

    public Exception? Exception { get; }
    public bool IsSuccess { get; }

    internal Result(bool isSuccess, Exception? exception = null)
    {
        IsSuccess = isSuccess;
        Exception = exception;
    }
}

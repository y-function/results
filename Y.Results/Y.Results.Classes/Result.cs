namespace Y.Results;

#if CLASSES
public partial class Result : IResultMarker
#elif STRUCTS
public readonly partial struct Result : IResultMarker
#endif
{
    public static bool TryGetFailure(out Result failure, params ReadOnlySpan<Result> results)
    {
        return TryGetFailureImpl(out failure, results);
    }

    public static bool TryGetFailure<T>(out Result<T> failure, params ReadOnlySpan<Result<T>> results)
    {
        return TryGetFailureImpl(out failure, results);
    }

    public static bool TryGetFailure<TOut>(out Result<TOut> failure, params ReadOnlySpan<Result> results)
    {
        return TryGetFailureImpl(out failure, results);
    }

    private static bool TryGetFailureImpl<TIn, TOut>(out TOut failure, params ReadOnlySpan<TIn> results) 
        where TIn : IResultMarker 
        where TOut : IResultMarker
    {
        foreach (ref readonly var result in results)
        {
            if (result.IsSuccess) continue;

            failure = result.GetType() == typeof(TOut) ? (TOut)(IResultMarker)result : (TOut)TOut.Failure(result.Exception!);
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

using System.Runtime.CompilerServices;

namespace Y.Results;

#if CLASSES
public partial class Result : IResultMarker
#elif STRUCTS
public readonly partial struct Result : IResultMarker
#endif
{
    public static bool TryGetFailure(out Result failure, params ReadOnlySpan<Result> results)
    {
        return TryGetFailureImpl(Failure, out failure, results);
    }

    public static bool TryGetFailure<T>(out Result<T> failure, params ReadOnlySpan<Result<T>> results)
    {
        return TryGetFailureImpl(Failure<T>, out failure, results);
    }

    public static bool TryGetFailure<TOut>(out Result<TOut> failure, params ReadOnlySpan<Result> results)
    {
        return TryGetFailureImpl(Failure<TOut>, out failure, results);
    }

    private static bool TryGetFailureImpl<TIn, TOut>(Func<Exception, TOut> failureBuilder, out TOut failure, params ReadOnlySpan<TIn> results)
#if CLASSES
        where TIn : class, IResultMarker 
        where TOut : class, IResultMarker
#elif STRUCTS
        where TIn : struct, IResultMarker
        where TOut : struct, IResultMarker
#endif
    {
        foreach (ref readonly var result in results)
        {
            if (result.IsSuccess) continue;

            failure = result.GetType() == typeof(TOut) ? Cast<TIn,TOut>(result) : failureBuilder(result.Exception!);
            return true;
        }

        failure = default!;
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if CLASSES
    private static TOut Cast<TIn, TOut>(TIn value) where TIn : class where TOut : class => Unsafe.As<TIn, TOut>(ref value);
#elif STRUCTS
    private static TOut Cast<TIn, TOut>(TIn value) where TIn : struct where TOut : struct => Unsafe.As<TIn, TOut>(ref value);
#endif

    public Exception? Exception { get; }
    public bool IsSuccess { get; }

    internal Result(bool isSuccess, Exception? exception = null)
    {
        IsSuccess = isSuccess;
        Exception = exception;
    }
}

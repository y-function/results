using System.Runtime.CompilerServices;

namespace Y.Results;

public static partial class ResultExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Result<TOut> CastFailure<TIn, TOut>(this Result<TIn> result) =>
#if CLASSES
        typeof(TIn) == typeof(TOut)
            ? (result as Result<TOut>)!
            : Result.Failure<TOut>(result.Exception!);
#elif STRUCTS
        Result.Failure<TOut>(result.Exception!);
#endif
}
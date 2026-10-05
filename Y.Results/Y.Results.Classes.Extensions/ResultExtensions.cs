using System.Runtime.CompilerServices;

namespace Y.Results;

public static partial class ResultExtensions
{
    private static Result<TOut> CastFailure<TIn, TOut>(this Result<TIn> result) =>
        typeof(TIn) == typeof(TOut)
#if CLASSES
            ? (result as Result<TOut>)! 
#elif STRUCTS
            ? Unsafe.As<Result<TIn>, Result<TOut>>(ref result)
#endif
            : Result.Failure<TOut>(result.Exception!);
}
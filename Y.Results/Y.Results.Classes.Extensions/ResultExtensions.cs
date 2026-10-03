using System.Runtime.CompilerServices;

namespace Y.Results;

public static partial class ResultExtensions
{
    private static Result<TOut> CastFailure<TIn, TOut>(this Result<TIn> result) => 
        typeof(TIn) == typeof(TOut) ? (result as Result<TOut>)! : Result.Failure<TOut>(result.Exception!);

    private static void AssertNotDefault(Result result, [CallerArgumentExpression(nameof(result))] string? paramName = null)
    {
#if CLASSES
        if (result == null)
#elif STRUCTS
        if (result == default)
#endif
        throw new ArgumentNullException(paramName);
    }
}
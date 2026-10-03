using System.Runtime.CompilerServices;

namespace Y.Results;

public static partial class ResultExtensions
{
    private static Result<TOut> CastFailure<TIn, TOut>(this Result<TIn> result) => 
        typeof(TIn) == typeof(TOut) ? (result as Result<TOut>)! : Result.Failure<TOut>(result.Exception!);
}
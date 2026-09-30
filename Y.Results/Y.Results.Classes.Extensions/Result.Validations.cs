namespace Y.Results;

public static partial class ResultExtensions
{
    public static void ThrowIfFailed(this Result? result)
    {
        if (!result?.IsSuccess ?? false)
            throw result.Exception!;
    }

    public static T GetValueOrThrow<T>(this Result<T>? result) =>
        result is null
            ? throw new ArgumentNullException(nameof(result))
            : !result.IsSuccess
                ? throw result.Exception!
                : result.Value!;

    public static Result<T> Ensure<T>(this Result<T>? result, Func<T, bool>? predicate, Func<T, Exception> onFailure)
    {
        if (predicate is null)
            return Result.Failure<T>(new ArgumentNullException(nameof(predicate)));

        return result.Then(v => predicate(v)
            ? Result.Success(v)
            : Result.Failure<T>(onFailure(v)));
    }

    public static Result<T> Ensure<T>(this Result<T>? result, Func<T, bool>? predicate, Func<Exception> onFailure) => 
        result.Ensure(predicate, _ => onFailure());

    public static Result Ensure(this Result? result, Func<bool>? predicate, Func<Exception> onFailure)
    {
        if (predicate is null)
            return Result.Failure(new ArgumentNullException(nameof(predicate)));

        return result.Then(() => predicate()
            ? Result.Success()
            : Result.Failure(onFailure()));
    }

    public static Result<T> FailIfNull<T>(this Result<T>? result, Func<Exception> onFailure) =>
        result.Ensure(value => value is not null, onFailure);
}
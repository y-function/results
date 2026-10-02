namespace Y.Results;

public static partial class ResultExtensions
{
    public static void ThrowIfFailed(this Result result)
    {
        ArgumentNullException.ThrowIfNull(result);
        if (!result.IsSuccess)
            throw result.Exception!;
    }

    public static T GetValueOrThrow<T>(this Result<T> result)
    {
        ArgumentNullException.ThrowIfNull(result);

        return !result.IsSuccess
            ? throw result.Exception!
            : result.Value!;
    }

    /// <summary>
    /// Invokes <paramref name="predicate"/> if the <paramref name="result"/> is success.
    /// Returns new <see cref="Result.Failure"/> if the <paramref name="predicate"/> resolves to <value>false</value>.
    /// If <paramref name="onFailure"/> is specified, invokes it to initialize the <see cref="Result.Failure"/> with its returned value.
    /// Otherwise, returns the original <paramref name="result"/>.
    /// </summary>
    public static Result Validate(this Result result, Func<bool> predicate, Func<Exception>? onFailure = null)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        return result.Then(() => Result.FromCondition(predicate, onFailure)).Then(result);
    }

    /// <summary>
    /// Invokes <paramref name="predicate"/> if the <paramref name="result"/> is success.
    /// Returns new <see cref="Result.Failure"/> if the <paramref name="predicate"/> resolves to <value>false</value>.
    /// If <paramref name="onFailure"/> is specified, invokes it to initialize the <see cref="Result.Failure"/> with its returned value.
    /// Otherwise, returns the original <paramref name="result"/>.
    /// </summary>
    public static Result<T> Validate<T>(this Result<T> result, Func<T, bool> predicate, Func<T, Exception>? onFailure = null)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        return result.Then(v => Result.FromCondition(v, predicate, onFailure)).Then(result);
    }

    /// <summary>
    /// Invokes <paramref name="predicate"/> if the <paramref name="result"/> is success.
    /// Returns new <see cref="Result.Failure"/> if the <paramref name="predicate"/> resolves to <value>false</value>.
    /// If <paramref name="onFailure"/> is specified, invokes it to initialize the <see cref="Result.Failure"/> with its returned value.
    /// Otherwise, returns the original <paramref name="result"/>.
    /// </summary>
    public static Result<T> Validate<T>(this Result<T> result, Func<T, bool> predicate, Func<Exception>? onFailure) =>
        Validate(result, predicate, onFailure is null ? null : _ => onFailure());

    /// <summary>
    /// Invokes <see cref="Validate"/> with the following predicate:<code>v => v is not null</code>. 
    /// </summary>
    public static Result<T> ValidateNotNull<T>(this Result<T> result, Func<Exception>? onFailure = null) => 
        Validate(result, v => v is not null, onFailure);
}
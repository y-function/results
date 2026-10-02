namespace Y.Results;

public static partial class ResultExtensions
{
    /// <summary>
    /// A transparent filter that executes the given <paramref name="action"/> if, and only if, the <paramref name="result"/> is Success.
    /// The <paramref name="action"/> execution result is swallowed. If you wish it to be returned, use <see cref="Then(Y.Results.Result?,System.Action?)"/> instead. 
    /// </summary>
    public static Result OnSuccess(this Result result, Action action)
    {
        ArgumentNullException.ThrowIfNull(result);
        
        if (!result.IsSuccess)
            return result;

        Result.FromAction(action);
        return result;
    }

    /// <summary>
    /// A transparent filter that executes the given <paramref name="action"/> if, and only if, the <paramref name="result"/> is Success.
    /// The <paramref name="action"/> execution result is swallowed. If you wish it to be returned, use <see cref="Then(Y.Results.Result?,System.Action?)"/> instead. 
    /// </summary>
    public static Result<T> OnSuccess<T>(this Result<T> result, Action<T> action)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(action);

        if (!result.IsSuccess)
            return result;

        Result.FromAction(() => action(result.Value));
        return result;
    }

    /// <summary>
    /// A transparent filter that executes the given <paramref name="action"/> if, and only if, the <paramref name="result"/> is Success.
    /// The <paramref name="action"/> execution result is swallowed. If you wish it to be returned, use <see cref="Then(Y.Results.Result?,System.Action?)"/> instead. 
    /// </summary>
    public static Result OnFailure(this Result result, Action<Exception> action)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(action);

        if (result.IsSuccess)
            return result;

        Result.FromAction(() => action(result.Exception!));
        return result;
    }

    /// <summary>
    /// A transparent filter that executes the given <paramref name="action"/> if, and only if, the <paramref name="result"/> is Success.
    /// The <paramref name="action"/> execution result is swallowed. If you wish it to be returned, use <see cref="Then(Y.Results.Result?,System.Action?)"/> instead. 
    /// </summary>
    public static Result<T> OnFailure<T>(this Result<T>? result, Action<Exception> action)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(action);

        if (result.IsSuccess)
            return result;

        Result.FromAction(() => action(result.Exception!));
        return result;
    }
}
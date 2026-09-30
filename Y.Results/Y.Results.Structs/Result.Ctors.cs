namespace Y.Results.Structs;

public readonly partial struct Result
{
    private const string ConditionValidationHasFailedErrorMessage = "Condition validation has failed.";

    public static Result Success() => new(true);

    public static Result<T> Success<T>(T value) => new(true, value);

    public static Result Failure(Exception exception) => new(false, exception);

    public static Result<T> Failure<T>(Exception exception) => new(false, default!, exception);

    public static Result FromAction(Action action)
    {
        ArgumentNullException.ThrowIfNull(action);

        try
        {
            action();
            return Success();
        }
        catch (Exception e)
        {
            return Failure(e);
        }
    }

    public static async Task<Result> FromAction(Func<Task> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        try
        {
            await action();
            return Success();
        }
        catch (Exception e)
        {
            return Failure(e);
        }
    }

    public static Result<T> FromAction<T>(Func<T> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        try
        {
            return Success(action());
        }
        catch (Exception e)
        {
            return Failure<T>(e);
        }
    }

    public static async Task<Result<T>> FromAction<T>(Func<Task<T>>? action)
    {
        ArgumentNullException.ThrowIfNull(action);

        try
        {
            return Success(await action());
        }
        catch (Exception e)
        {
            return Failure<T>(e);
        }
    }

    public static Result<T> FromCondition<T>(T o, Func<T, bool> predicate, string? errorMessage = null)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        
        try
        {
            return predicate(o)
                ? Success(o)
                : Failure<T>(new Exception(errorMessage ?? ConditionValidationHasFailedErrorMessage));
        }
        catch (Exception e)
        {
            return Failure<T>(e);
        }
    }
}
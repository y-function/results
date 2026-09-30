namespace Y.Results;

public partial class Result
{
    private const string ConditionValidationHasFailedErrorMessage = "Condition validation has failed.";

    public static Result Success() => new(true);

    public static Result<T> Success<T>(T value) => new(true, value);

    public static Result Failure(Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);
        return new Result(false, exception);
    }

    public static Result<T> Failure<T>(Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);

        return new Result<T>(false, default!, exception);
    }

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

    public static Result<T> FromFunction<T>(Func<T> func)
    {
        ArgumentNullException.ThrowIfNull(func);

        try
        {
            var value = func();
            return Success(value);
        }
        catch (Exception e)
        {
            return Failure<T>(e);
        }
    }

    public static async Task<Result<T>> FromFunction<T>(Func<Task<T>> func)
    {
        ArgumentNullException.ThrowIfNull(func);

        try
        {
            var value = await func();
            return Success(value);
        }
        catch (Exception e)
        {
            return Failure<T>(e);
        }
    }
    
    public static Result<T> FromCondition<T>(T o, Func<T, bool> predicate, Func<Exception>? onFailure = null)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        Exception GetException() => 
            onFailure?.Invoke() ?? new Exception(ConditionValidationHasFailedErrorMessage);
        
        try
        {
            return predicate(o) ? Success(o) : Failure<T>(GetException());
        }
        catch (Exception e)
        {
            return Failure<T>(e);
        }
    }
}
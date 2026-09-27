using System;
using System.Threading.Tasks;

namespace Y.Results;

public partial class Result
{
    private const string ConditionValidationHasFailedErrorMessage = "Condition validation has failed.";

    public static Result Success() => new(true);

    public static Result<T> Success<T>(T? value) => new(true, value);

    public static Result Failure(Exception exception) => new(false, exception);

    public static Result<T> Failure<T>(Exception exception) => new(false, default, exception);

    public static Result FromAction(Action? action)
    {
        if (action is null)
            return Failure(new ArgumentNullException(nameof(action)));
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

    public static async Task<Result> FromAction(Func<Task>? action)
    {
        if (action is null)
            return Failure(new ArgumentNullException(nameof(action)));

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

    public static Result<T> FromFunction<T>(Func<T>? func)
    {
        if (func is null)
            return Failure<T>(new ArgumentNullException(nameof(func)));

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

    public static async Task<Result<T>> FromFunction<T>(Func<Task<T>>? func)
    {
        if (func is null)
            return Failure<T>(new ArgumentNullException(nameof(func)));

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
    
    public static Result<T> FromCondition<T>(T o, Func<T, bool> predicate, string? errorMessage = null)
    {
        try
        {
            return predicate(o) ? Success(o) : Failure<T>(new Exception(errorMessage ?? ConditionValidationHasFailedErrorMessage));
        }
        catch (Exception e)
        {
            return Failure<T>(e);
        }
    }
}
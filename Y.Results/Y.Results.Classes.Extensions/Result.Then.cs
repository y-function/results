namespace Y.Results;

public static partial class ResultExtensions
{
    public static Result Then(this Result result, Action action)
    {
        ArgumentNullException.ThrowIfNull(result);

        return result.IsSuccess 
            ? Result.FromAction(action) 
            : result;
    }

    public static Result<TOut> Then<TOut>(this Result result, Func<TOut> action)
    {
        ArgumentNullException.ThrowIfNull(result);

        return result.IsSuccess 
            ? Result.FromFunction(action) 
            : Result.Failure<TOut>(result.Exception!);
    }

    public static Result Then(this Result result, Func<Result> action)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(action);
        if (!result.IsSuccess)
            return result;

        try
        {
            return action();
        }
        catch (Exception e)
        {
            return Result.Failure(e);
        }
    }

    public static Result<TOut> Then<TOut>(this Result result, Func<Result<TOut>> action)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(action);

        if (!result.IsSuccess)
            return Result.Failure<TOut>(result.Exception!);

        try
        {
            return action();
        }
        catch (Exception e)
        {
            return Result.Failure<TOut>(e);
        }
    }

    public static Result<TOut> Then<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> action)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(action);

        return result.IsSuccess 
            ? Result.FromFunction(() => action(result.Value)) 
            : Result.Failure<TOut>(result.Exception!);
    }

    public static Result<TOut> Then<TIn, TOut>(this Result<TIn> result, Func<TIn, Result<TOut>> action)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(action);

        if (!result.IsSuccess)
            return Result.Failure<TOut>(result.Exception!);

        try
        {
            return action(result.Value);
        }
        catch (Exception e)
        {
            return Result.Failure<TOut>(e);
        }
    }

    public static Result Then<TIn>(this Result<TIn> result, Func<TIn, Result> action)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(action);

        if (!result.IsSuccess)
            return result;

        try
        {
            return action(result.Value);
        }
        catch (Exception e)
        {
            return Result.Failure(e);
        }
    }
}
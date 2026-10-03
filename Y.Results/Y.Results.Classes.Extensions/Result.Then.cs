namespace Y.Results;

public static partial class ResultExtensions
{
    public static Result Then(this Result result, Result next)
    {
        AssertNotDefault(result);
        AssertNotDefault(next);
        
        return result.IsSuccess ? next : result;
    }

    public static Result Then(this Result result, Action action)
    {
        AssertNotDefault(result);

        return result.IsSuccess 
            ? Result.Wrap(action) 
            : result;
    }

    public static Result<TOut> Then<TOut>(this Result result, Func<TOut> action)
    {
        AssertNotDefault(result);

        return result.IsSuccess 
            ? Result.Wrap(action) 
            : Result.Failure<TOut>(result.Exception!);
    }

    public static Result Then(this Result result, Func<Result> action)
    {
        AssertNotDefault(result);
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

    public static Result<TOut> Then<TOut>(this Result result, Result<TOut> next)
    {
        AssertNotDefault(result);
        AssertNotDefault(next);

        return result.IsSuccess ? next : Result.Failure<TOut>(result.Exception!);
    }

    public static Result<TOut> Then<TIn, TOut>(this Result<TIn> result, Result<TOut> next)
    {
        AssertNotDefault(result);
        AssertNotDefault(next);

        return result.IsSuccess ? next : result.CastFailure<TIn, TOut>();
    }
    
    public static Result<TOut> Then<TOut>(this Result result, Func<Result<TOut>> action)
    {
        AssertNotDefault(result);
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
        AssertNotDefault(result);
        ArgumentNullException.ThrowIfNull(action);

        return result.IsSuccess 
            ? Result.Wrap(() => action(result.Value)) 
            : result.CastFailure<TIn, TOut>();
    }

    public static Result<TOut> Then<TIn, TOut>(this Result<TIn> result, Func<TIn, Result<TOut>> action)
    {
        AssertNotDefault(result);
        ArgumentNullException.ThrowIfNull(action);

        if (!result.IsSuccess)
            return result.CastFailure<TIn, TOut>();

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
        AssertNotDefault(result);
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
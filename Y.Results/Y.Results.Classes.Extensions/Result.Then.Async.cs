namespace Y.Results;

public static partial class ResultExtensions
{
    public static async Task<Result> Then(this Result result, Func<Task> action)
    {
        Result.AssertNotDefault(result);

        return result.IsSuccess 
            ? await Result.FromAction(action) 
            : result;
    }

    public static async Task<Result> Then<TIn>(this Result<TIn> result, Func<TIn, Task> action)
    {
        Result.AssertNotDefault(result);
        ArgumentNullException.ThrowIfNull(action);

        return result.IsSuccess 
            ? await Result.FromAction(() => action(result.Value)) 
            : result;
    }

    public static async Task<Result<TOut>> Then<TOut>(this Result result, Func<Task<Result<TOut>>> action)
    {
        Result.AssertNotDefault(result);
        ArgumentNullException.ThrowIfNull(action);

        if (!result.IsSuccess)
            return Result.Failure<TOut>(result.Exception!);

        try
        {
            return await action();
        }
        catch (Exception e)
        {
            return Result.Failure<TOut>(e);
        }
    }

    public static async Task<Result> Then(this Result result, Func<Task<Result>> action)
    {
        Result.AssertNotDefault(result);
        ArgumentNullException.ThrowIfNull(action);

        if (!result.IsSuccess)
            return result;

        try
        {
            return await action();
        }
        catch (Exception e)
        {
            return Result.Failure(e);
        }
    }

    public static async Task<Result> Then<TIn>(this Result<TIn> result, Func<TIn, Task<Result>> action)
    {
        Result.AssertNotDefault(result);
        ArgumentNullException.ThrowIfNull(action);

        if (!result.IsSuccess)
            return result;

        try
        {
            return await action(result.Value);
        }
        catch (Exception e)
        {
            return Result.Failure(e);
        }
    }

    public static async Task<Result<TOut>> Then<TIn, TOut>(this Result<TIn> result, Func<TIn, Task<TOut>> action)
    {
        Result.AssertNotDefault(result);
        ArgumentNullException.ThrowIfNull(action);

        return result.IsSuccess
            ? await Result.FromFunction(() => action(result.Value))
            : Result.Failure<TOut>(result.Exception!);
    }

    public static async Task<Result<TOut>> Then<TIn, TOut>(this Result<TIn> result, Func<TIn, Task<Result<TOut>>> action)
    {
        Result.AssertNotDefault(result);
        ArgumentNullException.ThrowIfNull(action);

        if (!result.IsSuccess)
            return Result.Failure<TOut>(result.Exception!);

        try
        {
            return await action(result.Value);
        }
        catch (Exception e)
        {
            return Result.Failure<TOut>(e);
        }
    }
}
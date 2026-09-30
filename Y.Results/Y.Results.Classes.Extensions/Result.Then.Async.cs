namespace Y.Results;

public static partial class ResultExtensions
{
    public static async Task<Result> Then(this Result? result, Func<Task>? action)
    {
        if (result is null)
            return Result.Failure(new ArgumentNullException(nameof(result)));
        if (!result.IsSuccess)
            return Result.Failure(result.Exception!);
        if (action is null)
            return Result.Failure(new ArgumentNullException(nameof(action)));

        try
        {
            await action();
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e);
        }
    }

    public static async Task<Result> Then<TIn>(this Result<TIn>? result, Func<TIn, Task>? action)
    {
        if (result is null)
            return Result.Failure(new ArgumentNullException(nameof(result)));
        if (!result.IsSuccess)
            return Result.Failure(result.Exception!);
        if (action is null)
            return Result.Failure(new ArgumentNullException(nameof(action)));

        try
        {
            await action(result.Value);
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e);
        }
    }

    public static async Task<Result<TOut>> Then<TOut>(this Result? result, Func<Task<Result<TOut>>>? action)
    {
        if (result is null)
            return Result.Failure<TOut>(new ArgumentNullException(nameof(result)));
        if (!result.IsSuccess)
            return Result.Failure<TOut>(result.Exception!);
        if (action is null)
            return Result.Failure<TOut>(new ArgumentNullException(nameof(action)));

        try
        {
            var actionResult = await action();
            return actionResult;
        }
        catch (Exception e)
        {
            return Result.Failure<TOut>(e);
        }
    }

    public static async Task<Result> Then(this Result? result, Func<Task<Result>>? action)
    {
        if (result is null)
            return Result.Failure(new ArgumentNullException(nameof(result)));
        if (!result.IsSuccess)
            return Result.Failure(result.Exception!);
        if (action is null)
            return Result.Failure(new ArgumentNullException(nameof(action)));

        try
        {
            var actionResult = await action();
            return actionResult;
        }
        catch (Exception e)
        {
            return Result.Failure(e);
        }
    }

    public static async Task<Result> Then<TIn>(this Result<TIn>? result, Func<TIn?, Task<Result>>? mapper)
    {
        if (result is null)
            return Result.Failure(new ArgumentNullException(nameof(result)));
        if (!result.IsSuccess)
            return Result.Failure(result.Exception!);
        if (mapper is null)
            return Result.Failure(new ArgumentNullException(nameof(mapper)));

        try
        {
            var valueResult = await mapper(result.Value);
            return valueResult;
        }
        catch (Exception e)
        {
            return Result.Failure(e);
        }
    }

    public static async Task<Result<TOut>> Then<TIn, TOut>(this Result<TIn>? result, Func<TIn?, Task<TOut>>? mapper)
    {
        if (result is null)
            return Result.Failure<TOut>(new ArgumentNullException(nameof(result)));
        if (!result.IsSuccess)
            return Result.Failure<TOut>(result.Exception!);
        if (mapper is null)
            return Result.Failure<TOut>(new ArgumentNullException(nameof(mapper)));

        try
        {
            var value = await mapper(result.Value);
            return Result.Success(value);
        }
        catch (Exception e)
        {
            return Result.Failure<TOut>(e);
        }
    }

    public static async Task<Result<TOut>> Then<TIn, TOut>(
        this Result<TIn>? result,
        Func<TIn?, Task<Result<TOut>>>? mapper)
    {
        if (result is null)
            return Result.Failure<TOut>(new ArgumentNullException(nameof(result)));
        if (!result.IsSuccess)
            return Result.Failure<TOut>(result.Exception!);
        if (mapper is null)
            return Result.Failure<TOut>(new ArgumentNullException(nameof(mapper)));

        try
        {
            var resultValue = await mapper(result.Value);
            return resultValue;
        }
        catch (Exception e)
        {
            return Result.Failure<TOut>(e);
        }
    }
}
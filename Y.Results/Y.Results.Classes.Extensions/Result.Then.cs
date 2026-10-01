namespace Y.Results;

public static partial class ResultExtensions
{
    public static Result Then(this Result result, Action action)
    {
        ArgumentNullException.ThrowIfNull(result);

        return !result.IsSuccess ? result : Result.FromAction(action);
    }

    public static Result<TOut> Then<TOut>(this Result? result, Func<TOut>? mapper)
    {
        if (result is null)
            return Result.Failure<TOut>(new ArgumentNullException(nameof(result)));
        if (!result.IsSuccess)
            return Result.Failure<TOut>(result.Exception!);
        if (mapper is null)
            return Result.Failure<TOut>(new ArgumentNullException(nameof(mapper)));

        return Result.FromFunction(mapper);
    }

    public static Result Then(this Result? result, Func<Result>? mapper)
    {
        if (result is null)
            return Result.Failure(new ArgumentNullException(nameof(result)));
        if (!result.IsSuccess)
            return Result.Failure(result.Exception!);
        if (mapper is null)
            return Result.Failure(new ArgumentNullException(nameof(mapper)));

        return mapper();
    }

    public static Result<TOut> Then<TOut>(this Result? result, Func<Result<TOut>>? action)
    {
        if (result is null)
            return Result.Failure<TOut>(new ArgumentNullException(nameof(result)));
        if (!result.IsSuccess)
            return Result.Failure<TOut>(result.Exception!);
        if (action is null)
            return Result.Failure<TOut>(new ArgumentNullException(nameof(action)));

        try
        {
            var actionResult = action();
            return actionResult;
        }
        catch (Exception e)
        {
            return Result.Failure<TOut>(e);
        }
    }

    public static Result<TOut> Then<TIn, TOut>(this Result<TIn>? result, Func<TIn?, TOut>? mapper)
    {
        if (result is null)
            return Result.Failure<TOut>(new ArgumentNullException(nameof(result)));
        if (!result.IsSuccess)
            return Result.Failure<TOut>(result.Exception!);
        if (mapper is null)
            return Result.Failure<TOut>(new ArgumentNullException(nameof(mapper)));

        try
        {
            var value = mapper(result.Value);
            return Result.Success(value);
        }
        catch (Exception e)
        {
            return Result.Failure<TOut>(e);
        }
    }

    public static Result<TOut> Then<TIn, TOut>(this Result<TIn>? result, Func<TIn?, Result<TOut>>? mapper)
    {
        if (result is null)
            return Result.Failure<TOut>(new ArgumentNullException(nameof(result)));
        if (!result.IsSuccess)
            return Result.Failure<TOut>(result.Exception!);
        if (mapper is null)
            return Result.Failure<TOut>(new ArgumentNullException(nameof(mapper)));

        try
        {
            var valueResult = mapper(result.Value);
            return valueResult;
        }
        catch (Exception e)
        {
            return Result.Failure<TOut>(e);
        }
    }

    public static Result Then<TIn>(this Result<TIn>? result, Func<TIn?, Result>? mapper)
    {
        if (result is null)
            return Result.Failure(new ArgumentNullException(nameof(result)));
        if (!result.IsSuccess)
            return Result.Failure(result.Exception!);
        if (mapper is null)
            return Result.Failure(new ArgumentNullException(nameof(mapper)));

        try
        {
            var valueResult = mapper(result.Value);
            return valueResult;
        }
        catch (Exception e)
        {
            return Result.Failure(e);
        }
    }
}
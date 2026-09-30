namespace Y.Results.Structs;

public readonly partial struct Result
{
    private const string OperationFailedErrorMessage = "Operation failed.";

    public static bool TryGetFailedResult(out Result failedResult, params ReadOnlySpan<Result> results)
    {
        foreach (ref readonly var result in results)
        {
            if (result.IsSuccess) continue;
            failedResult = result;
            return true;
        }

        failedResult = default;
        return false;
    }

    public static bool TryGetFailedResult<T>(out Result<T> failedResult, params ReadOnlySpan<Result<T>> results)
    {
        foreach (ref readonly var result in results)
        {
            if (result.IsSuccess) continue;
            failedResult = result;
            return true;
        }

        failedResult = default;
        return false;
    }

    public static bool TryGetFailedResult<TOut>(out Result<TOut> failedResult, params ReadOnlySpan<Result> results)
    {
        foreach (ref readonly var result in results)
        {
            if (result.IsSuccess) continue;
            failedResult = Failure<TOut>(result.Exception ?? new Exception(OperationFailedErrorMessage));
            return true;
        }

        failedResult = default;
        return false;
    }

    public Exception Exception { get; }
    public bool IsSuccess { get; }

    internal Result(bool isSuccess, Exception? exception = null)
    {
        IsSuccess = isSuccess;
        Exception = exception!;
    }
}
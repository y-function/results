using System;
using System.Linq;

namespace Y.Results;

public partial class Result
{
    public static bool TryGetFailure(out Result? failure, params Result[] results)
    {
        failure = results.FirstOrDefault(x => !x.IsSuccess);
        return failure is not null;
    }

    public static bool TryGetFailure<T>(out Result<T>? failure, params Result<T>[] results)
    {
        failure = results.FirstOrDefault(x => !x.IsSuccess);
        return failure is not null;
    }

    public static bool TryGetFailure<TOut>(out Result<TOut>? failure, params Result[] results)
    {
        var failed = results.FirstOrDefault(x => !x.IsSuccess && x.Exception is not null);
        if (failed is null)
        {
            failure = null;
            return false;
        }

        failure = failed is Result<TOut> r ? r : Failure<TOut>(failed.Exception!);
        return true;
    }

    public Exception? Exception { get; }
    public bool IsSuccess { get; }

    protected Result(bool isSuccess, Exception? exception = null)
    {
        IsSuccess = isSuccess;
        Exception = exception;
    }
}

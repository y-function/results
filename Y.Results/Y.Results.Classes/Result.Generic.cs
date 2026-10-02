namespace Y.Results;

#if CLASSES
public class Result<T> : Result, IResultMarker
#elif STRUCTS
public readonly struct Result<T> : IResultMarker
#endif
{
    static IResultMarker IResultMarker.Failure(Exception exception)
    {
        return Result.Failure<T>(exception);
    }
 
    public T Value { get; }
#if CLASSES
    protected internal Result(bool isSuccess, T value, Exception? exception = null) : base(isSuccess, exception) => Value = value;
#elif STRUCTS
    public static implicit operator Result(Result<T> result) 
        => new(result.IsSuccess, result.Exception);

    public Exception Exception { get; }
    public bool IsSuccess { get; }

    internal Result(bool isSuccess, T value = default!, Exception? exception = null)
    {
        IsSuccess = isSuccess;
        Value = value;
        Exception = exception!;
    }
#endif
}
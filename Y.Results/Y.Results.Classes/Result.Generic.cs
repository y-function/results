namespace Y.Results;

#if CLASSES
public class Result<T> : Result
#elif STRUCTS
public readonly record struct Result<T>
#endif
{
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
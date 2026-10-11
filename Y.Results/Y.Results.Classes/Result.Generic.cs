namespace Y.Results;

#if CLASSES
public class Result<T> : Result, IResult
#elif STRUCTS
public readonly record struct Result<T> : IResult
#endif
{
    public T Value { get; }
    object? IResult.Value => Value;
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
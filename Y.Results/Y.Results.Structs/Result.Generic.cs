namespace Y.Results;

public readonly struct Result<T>
{
    public static implicit operator Result(Result<T> result) 
        => new(result.IsSuccess, result.Exception);

    public Exception Exception { get; }
    public bool IsSuccess { get; }
    public T Value { get; }

    internal Result(bool isSuccess, T value = default!, Exception? exception = null)
    {
        IsSuccess = isSuccess;
        Value = value;
        Exception = exception!;
    }
}
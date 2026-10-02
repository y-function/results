namespace Y.Results;

public class Result<T> : Result
{
    public T Value { get; }

    protected internal Result(bool isSuccess, T value, Exception? exception = null) : base(isSuccess, exception) => Value = value;
}
namespace Y.Results;

#if CLASSES
public partial class Result : IResult
#elif STRUCTS
public readonly partial record struct Result : IResult
#endif
{
    public Exception? Exception { get; }
    public bool IsSuccess { get; }
    object? IResult.Value => null;

    internal Result(bool isSuccess, Exception? exception = null)
    {
        IsSuccess = isSuccess;
        Exception = exception;
    }
}

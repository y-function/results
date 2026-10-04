using System.Runtime.CompilerServices;

namespace Y.Results;

#if CLASSES
public partial class Result
#elif STRUCTS
public readonly partial record struct Result
#endif
{
    public Exception? Exception { get; }
    public bool IsSuccess { get; }

    internal Result(bool isSuccess, Exception? exception = null)
    {
        IsSuccess = isSuccess;
        Exception = exception;
    }
}

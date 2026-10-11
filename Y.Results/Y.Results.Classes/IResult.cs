namespace Y.Results;

internal interface IResult
{
    Exception? Exception { get; }
    bool IsSuccess { get; }
    object? Value { get; }
}

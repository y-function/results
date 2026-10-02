namespace Y.Results;

internal interface IResultMarker
{
    bool IsSuccess { get; }
    Exception? Exception { get; }

    static abstract IResultMarker Failure(Exception exception);
}
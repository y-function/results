namespace Y.Results;

#if CLASSES
public partial class Result
#elif STRUCTS
public readonly partial struct Result
#endif
{
    public static Result WhenAll(params ReadOnlySpan<Result> results) => 
        TryGetFailure(out var result,  results) ? result : Success();

    public static Result<IReadOnlyList<T>> WhenAll<T>(params ReadOnlySpan<Result<T>> results)
    {
        var values = new List<T>();
        foreach (ref readonly var result in results)
        {
            if (!result.IsSuccess) 
                return Failure<IReadOnlyList<T>>(result.Exception!);

            values.Add(result.Value);
        }

        return Success<IReadOnlyList<T>>(values);
    }
    
    public static Result<Tuple<T1, T2>> WhenAll<T1, T2>(Result<T1> r1, Result<T2> r2) =>
        !r1.IsSuccess
            ? Failure<Tuple<T1, T2>>(r1.Exception!)
            : !r2.IsSuccess
                ? Failure<Tuple<T1, T2>>(r2.Exception!)
                : Success(new Tuple<T1, T2>(r1.Value!, r2.Value!));

    public static Result<Tuple<T1, T2, T3>> WhenAll<T1, T2, T3>(Result<T1> r1, Result<T2> r2, Result<T3> r3) =>
        !r1.IsSuccess
            ? Failure<Tuple<T1, T2, T3>>(r1.Exception!)
            : !r2.IsSuccess
                ? Failure<Tuple<T1, T2, T3>>(r2.Exception!)
                : !r3.IsSuccess
                    ? Failure<Tuple<T1, T2, T3>>(r3.Exception!)
                    : Success(new Tuple<T1, T2, T3>(r1.Value!, r2.Value!, r3.Value!));
}
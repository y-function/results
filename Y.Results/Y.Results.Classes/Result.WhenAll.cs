namespace Y.Results;

#if CLASSES
public partial class Result
#elif STRUCTS
public readonly partial record struct Result
#endif
{
    public static Result WhenAll(
#if NETSTANDARD2_0
        params Result[] results
#else
        params ReadOnlySpan<Result> results
#endif
    ) => 
        TryGetFailure(out var result, results) ? result : Success();

    public static Result<IReadOnlyList<T>> WhenAll<T>(
#if NETSTANDARD2_0
        params Result<T>[] results
#else
        params ReadOnlySpan<Result<T>> results
#endif
    )
    {
        var values = new List<T>();
#if NETSTANDARD2_0
        foreach (var result in results ?? [])
#else
        foreach (ref readonly var result in results)
#endif
        {
            AssertNotDefault(result);
            if (!result.IsSuccess) 
                return Failure<IReadOnlyList<T>>(result.Exception!);

            values.Add(result.Value);
        }

        return Success<IReadOnlyList<T>>(values);
    }
    
    public static Result<Tuple<T1, T2>> WhenAll<T1, T2>(Result<T1> r1, Result<T2> r2)
    {
        AssertNotDefault(r1);
        AssertNotDefault(r2);

        return r1.IsSuccess
            ? r2.IsSuccess
                ? Success(new Tuple<T1, T2>(r1.Value!, r2.Value!))
                : Failure<Tuple<T1, T2>>(r2.Exception!)
            : Failure<Tuple<T1, T2>>(r1.Exception!);
    }

    public static Result<Tuple<T1, T2, T3>> WhenAll<T1, T2, T3>(Result<T1> r1, Result<T2> r2, Result<T3> r3)
    {
        AssertNotDefault(r1);
        AssertNotDefault(r2);
        AssertNotDefault(r3);

        return r1.IsSuccess
            ? r2.IsSuccess
                ? r3.IsSuccess
                    ? Success(new Tuple<T1, T2, T3>(r1.Value!, r2.Value!, r3.Value!))
                    : Failure<Tuple<T1, T2, T3>>(r3.Exception!)
                : Failure<Tuple<T1, T2, T3>>(r2.Exception!)
            : Failure<Tuple<T1, T2, T3>>(r1.Exception!);
    }
}
namespace Y.Results;

#if CLASSES
public partial class Result
#elif STRUCTS
public readonly partial struct Result
#endif
{
    /// <summary>
    /// Executes <paramref name="unsafeFunction"/> and returns <see cref="Success"/> or <see cref="Failure"/> depending on if it throws.
    /// An <see cref="ArgumentNullException"/> is thrown if <paramref name="unsafeFunction"/> is <see langword="null"/>. 
    /// </summary>
    public static Result Wrap(Action unsafeFunction)
    {
        ArgumentNullException.ThrowIfNull(unsafeFunction);

        try
        {
            unsafeFunction();
            return Success();
        }
        catch (Exception e)
        {
            return Failure(e);
        }
    }

    /// <summary>
    /// Executes <paramref name="unsafeFunction"/> and returns <see cref="Success"/> or <see cref="Failure"/> depending on if it throws.
    /// An <see cref="ArgumentNullException"/> is thrown if <paramref name="unsafeFunction"/> is <see langword="null"/>. 
    /// </summary>
    public static async Task<Result> Wrap(Func<Task> unsafeFunction)
    {
        ArgumentNullException.ThrowIfNull(unsafeFunction);

        try
        {
            await unsafeFunction();
            return Success();
        }
        catch (Exception e)
        {
            return Failure(e);
        }
    }

    /// <summary>
    /// Executes <paramref name="unsafeFunction"/> and returns <see cref="Success"/> or <see cref="Failure"/> depending on if it throws.
    /// An <see cref="ArgumentNullException"/> is thrown if <paramref name="unsafeFunction"/> is <see langword="null"/>. 
    /// </summary>
    public static Result<T> Wrap<T>(Func<T> unsafeFunction)
    {
        ArgumentNullException.ThrowIfNull(unsafeFunction);

        try
        {
            var value = unsafeFunction();
            return Success(value);
        }
        catch (Exception e)
        {
            return Failure<T>(e);
        }
    }

    /// <summary>
    /// Executes <paramref name="unsafeFunction"/> and returns <see cref="Success"/> or <see cref="Failure"/> depending on if it throws.
    /// An <see cref="ArgumentNullException"/> is thrown if <paramref name="unsafeFunction"/> is <see langword="null"/>. 
    /// </summary>
    public static async Task<Result<T>> Wrap<T>(Func<Task<T>> unsafeFunction)
    {
        ArgumentNullException.ThrowIfNull(unsafeFunction);

        try
        {
            var value = await unsafeFunction();
            return Success(value);
        }
        catch (Exception e)
        {
            return Failure<T>(e);
        }
    }
}
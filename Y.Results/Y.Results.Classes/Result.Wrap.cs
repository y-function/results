namespace Y.Results;

#if CLASSES
public partial class Result
#elif STRUCTS
public readonly partial record struct Result
#endif
{
    /// <summary>
    /// Executes <paramref name="unsafeOperation"/> and returns <see cref="Success"/> or <see cref="Failure"/> depending on if it throws.
    /// An <see cref="ArgumentNullException"/> is thrown if <paramref name="unsafeOperation"/> is <see langword="null"/>. 
    /// </summary>
    public static Result Wrap(Action unsafeOperation)
    {
        ArgumentNullException.ThrowIfNull(unsafeOperation);

        try
        {
            unsafeOperation();
            return Success();
        }
        catch (Exception e)
        {
            return Failure(e);
        }
    }

    /// <summary>
    /// Executes <paramref name="unsafeOperation"/> and returns <see cref="Success"/> or <see cref="Failure"/> depending on if it throws.
    /// An <see cref="ArgumentNullException"/> is thrown if <paramref name="unsafeOperation"/> is <see langword="null"/>. 
    /// </summary>
    public static async Task<Result> Wrap(Func<Task> unsafeOperation)
    {
        ArgumentNullException.ThrowIfNull(unsafeOperation);

        try
        {
            await unsafeOperation();
            return Success();
        }
        catch (Exception e)
        {
            return Failure(e);
        }
    }

    /// <summary>
    /// Executes <paramref name="unsafeOperation"/> and returns <see cref="Success"/> or <see cref="Failure"/> depending on if it throws.
    /// An <see cref="ArgumentNullException"/> is thrown if <paramref name="unsafeOperation"/> is <see langword="null"/>. 
    /// </summary>
    public static Result<T> Wrap<T>(Func<T> unsafeOperation)
    {
        ArgumentNullException.ThrowIfNull(unsafeOperation);

        try
        {
            var value = unsafeOperation();
            return Success(value);
        }
        catch (Exception e)
        {
            return Failure<T>(e);
        }
    }

    /// <summary>
    /// Executes <paramref name="unsafeOperation"/> and returns <see cref="Success"/> or <see cref="Failure"/> depending on if it throws.
    /// An <see cref="ArgumentNullException"/> is thrown if <paramref name="unsafeOperation"/> is <see langword="null"/>. 
    /// </summary>
    public static async Task<Result<T>> Wrap<T>(Func<Task<T>> unsafeOperation)
    {
        ArgumentNullException.ThrowIfNull(unsafeOperation);

        try
        {
            var value = await unsafeOperation();
            return Success(value);
        }
        catch (Exception e)
        {
            return Failure<T>(e);
        }
    }
}
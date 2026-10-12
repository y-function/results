#if NETSTANDARD2_0
using System.Runtime.CompilerServices;

namespace Y.Results;

[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
public class ArgumentNullException : System.ArgumentNullException
{
    public ArgumentNullException() { }

    public ArgumentNullException(string? paramName) : base(paramName) { }

    public ArgumentNullException(string? paramName, string? message) : base(paramName, message) { }

    public static void ThrowIfNull(object? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        if (argument is null)
            throw new ArgumentNullException(paramName);
    }
}
#endif

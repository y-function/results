namespace Y.Results;

#if CLASSES
public partial class Result
#elif STRUCTS
public readonly partial struct Result
#endif
    : IEquatable<Result>
{
#if CLASSES
    public bool Equals(Result? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
#elif STRUCTS
    public bool Equals(Result other)
    {
#endif

        return Equals(Exception, other.Exception) && IsSuccess == other.IsSuccess;
    }
    
    public override bool Equals(object? obj) => obj is Result other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(Exception, IsSuccess);

    public static bool operator ==(Result left, Result right) => Equals(left, right);

    public static bool operator !=(Result left, Result right) => !Equals(left, right);
}
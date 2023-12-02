namespace Domain.ValueTypes;

public class RealtorId : IEquatable<RealtorId>
{
    public readonly int Value;
    
    private RealtorId(int value)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(RealtorId)} must be above a positive number");
        }

        Value = value;
    }

    public static RealtorId New(int value) => new(value);

    public static RealtorId From(string value)
    {
        if (int.TryParse(value, out int result))
        {
            return new RealtorId(result);
        }

        throw new FormatException($"Unable to convert {nameof(value)} to {nameof(RealtorId)}");
    }
    
    public bool Equals(RealtorId? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((RealtorId)obj);
    }
    
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}
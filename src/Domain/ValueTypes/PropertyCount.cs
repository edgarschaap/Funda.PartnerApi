namespace Domain.ValueTypes;

public class PropertyCount : IEquatable<PropertyCount>
{
    public readonly int Value;
    
    private PropertyCount(int value)
    {
        if (value < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(PropertyCount)} must be above 1");
        }

        Value = value;
    }

    public static PropertyCount New(int value) => new(value);

    public static PropertyCount From(string value)
    {
        if (int.TryParse(value, out int result))
        {
            return new PropertyCount(result);
        }

        throw new FormatException($"Unable to convert {nameof(value)} to {nameof(PropertyCount)}");
    }
    
    public bool Equals(PropertyCount? other)
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
        return Equals((PropertyCount)obj);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}
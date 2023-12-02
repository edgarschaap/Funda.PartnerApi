namespace Domain.ValueTypes;

public class RealtorName : ConceptualString
{
    public override string Value { get; }
    
    public static implicit operator RealtorName(string realtorName) => new(realtorName);

    private RealtorName(string realtorName)
    {
        if (string.IsNullOrWhiteSpace(realtorName))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(realtorName));
        Value = realtorName;
    }

    public static RealtorName New(string realtorName) => new RealtorName(realtorName);
}
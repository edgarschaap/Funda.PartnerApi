namespace Domain.ValueTypes;

public class SearchKey : ConceptualString
{
    public override string Value { get; }
    
    public static implicit operator SearchKey(string searchKey) => new(searchKey);

    private SearchKey(string searchKey)
    {
        if (string.IsNullOrWhiteSpace(searchKey))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(searchKey));
        Value = searchKey;
    }

    public static SearchKey New(string searchKey) => new SearchKey(searchKey);
}
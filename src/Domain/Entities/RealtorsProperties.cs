using Domain.ValueTypes;

namespace Domain.Entities;

public class RealtorsProperties
{
    public RealtorName RealtorName { get; }
    public PropertyCount PropertyCount { get; }

    private RealtorsProperties(RealtorName realtorName, PropertyCount propertyCount)
    {
        RealtorName = realtorName ?? throw new ArgumentNullException(nameof(realtorName));
        PropertyCount = propertyCount ?? throw new ArgumentNullException(nameof(propertyCount));
    }
    
    public static RealtorsProperties New(RealtorName realtorName, PropertyCount propertyCount) => new(realtorName, propertyCount);
}
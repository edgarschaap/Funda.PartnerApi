using Domain.ValueTypes;

namespace Domain.Entities;

public class Realtor
{
    public RealtorId RealtorId { get; }
    public RealtorId RealtId { get; }
    public RealtorName RealtorName { get; }

    private Realtor(RealtorId realtorId, RealtorName realtorName)
    {
        RealtorId = realtorId ?? throw new ArgumentNullException(nameof(realtorId));
        RealtorName = realtorName ?? throw new ArgumentNullException(nameof(realtorName));
    }

    public static Realtor New(RealtorId realtorId, RealtorName realtorName) => new(realtorId, realtorName);
}
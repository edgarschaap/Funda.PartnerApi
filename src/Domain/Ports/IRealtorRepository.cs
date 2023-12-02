using Domain.Entities;
using Domain.ValueTypes;

namespace Domain.Ports;

public interface IRealtorRepository
{
    Task<IEnumerable<Realtor>> GetRealtorsForKeysAsync(IEnumerable<SearchKey> searchKeys, CancellationToken cancellationToken);
}
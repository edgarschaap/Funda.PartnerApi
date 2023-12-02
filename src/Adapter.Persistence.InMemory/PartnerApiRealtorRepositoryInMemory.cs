using System.Collections;
using Domain.Entities;
using Domain.Ports;
using Domain.ValueTypes;

namespace Adapter.Persistence.InMemory;

public class PartnerApiRealtorRepositoryInMemory : IRealtorRepository
{
    private readonly Dictionary<string, List<Realtor>> _realtorsBySearchKeys = new();

    public async Task<IEnumerable<Realtor>> GetRealtorsForKeysAsync(IEnumerable<SearchKey> searchKeys, CancellationToken cancellationToken)
    {
        var key = GetKey(searchKeys);

        if (_realtorsBySearchKeys.TryGetValue(key, out var realtors))
        {
            return await Task.FromResult(realtors);
        }

        return await Task.FromResult(new List<Realtor>());
    }

    public void AddRealtorForKeys(Realtor realtor, IEnumerable<SearchKey> searchKeys)
    {
        var key = GetKey(searchKeys);

        if (_realtorsBySearchKeys.ContainsKey(key) == false)
        {
            _realtorsBySearchKeys.Add(key, new List<Realtor>());
        }
        
        _realtorsBySearchKeys[key].Add(realtor);
    }
    
    
    private string GetKey(IEnumerable<SearchKey> searchKeys) => string.Join("|", searchKeys);
}
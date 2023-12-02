using Adapter.Http.PartnerApi.Mappers;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Ports;
using Domain.ValueTypes;
using PartnerApi.Client.Services.Search;

namespace Adapter.Http.PartnerApi;

public class PartnerApiRealtorRepository : IRealtorRepository
{
    private readonly IPropertySearchService _propertySearchService;

    public PartnerApiRealtorRepository(IPropertySearchService propertySearchService)
    {
        _propertySearchService = propertySearchService ?? throw new ArgumentNullException(nameof(propertySearchService));
    }
    
    public async Task<IEnumerable<Realtor>> GetRealtorsForKeysAsync(IEnumerable<SearchKey> searchKeys, CancellationToken cancellationToken)
    {
        var keys = searchKeys.Select(key => key.Value);
        
        var result = await _propertySearchService.GetPropertyRealtorsForKeysAsync(keys, cancellationToken);

        if (result.IsSuccessful)
        {
            return result.Value.Select(dto => dto.ToDomainEntity());
        }
        
        throw new PersistenceException(true, $"Failed to retrieve realtors for search key(s) {string.Join('|', keys)}");
    }
}
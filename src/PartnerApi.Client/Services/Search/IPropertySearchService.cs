using PartnerApi.Client.Dtos.Response;

namespace PartnerApi.Client.Services.Search;

public interface IPropertySearchService
{
    Task<IPartnerApiResult<IEnumerable<PropertyObjectDto>>> GetPropertyRealtorsForKeysAsync(IEnumerable<string> searchKeys, CancellationToken cancellationToken);
}
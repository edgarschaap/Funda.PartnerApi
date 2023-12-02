using PartnerApi.Client.Dtos.Response;
using PartnerApi.Client.Services.Search.Actions;
using Polly;
using Serilog;

namespace PartnerApi.Client.Services.Search;

internal class PropertySearchService : ClientServiceBase, IPropertySearchService
{
    public PropertySearchService(string baseUrl, string clientApplication, 
        AsyncPolicy resiliencePolicy, 
        PartnerApiHttpClientFactory httpClientFactory, ILogger logger) 
        : base(baseUrl, clientApplication, resiliencePolicy, httpClientFactory, logger)
    {
    }

    public async Task<IPartnerApiResult<IEnumerable<PropertyObjectDto>>> GetPropertyRealtorsForKeysAsync(IEnumerable<string> searchKeys, CancellationToken cancellationToken)
    {
        var action = new StartPropertyRealtorSearchAction(BaseUrl, ClientApplication, ResiliencePolicy, HttpClientFactory, searchKeys);

        var firstSearchResult = await action.ExecuteAsync(cancellationToken).ConfigureAwait(false);

        if (firstSearchResult.IsFailure) return PartnerApiResult<IEnumerable<PropertyObjectDto>>.ClientFailure(firstSearchResult.Message, firstSearchResult.Exception);
        
        var nextPage = firstSearchResult.Value.Paging.NextPage;
        
        var properties = new List<PropertyObjectDto>();
        properties.AddRange(firstSearchResult.Value.PropertyObjects);
        
        
        while (true)
        {
            var getNextPageAction = new NextPagePropertyRealtorSearchAction(BaseUrl, ClientApplication, ResiliencePolicy, HttpClientFactory, searchKeys, nextPage);

            var result = await getNextPageAction.ExecuteAsync(cancellationToken).ConfigureAwait(false);

            if (result.IsSuccessful)
            {
                properties.AddRange(result.Value.PropertyObjects);
                
                if (result.Value.Paging.IsLastPage) break;
            }

            nextPage++;
        }

        return PartnerApiResult<IEnumerable<PropertyObjectDto>>.Successful(properties);
    }
}
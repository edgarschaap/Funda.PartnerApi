using Flurl;
using PartnerApi.Client.Dtos.Response;
using PartnerApi.Client.Resilience;
using Polly;

namespace PartnerApi.Client.Services.Search.Actions;

internal class StartPropertyRealtorSearchAction : ResilientActionAsyncBase<PropertyRealtorSearchResultDto>
{
    private const int MaxPageSize = 25;
    
    private readonly IEnumerable<string> _searchKeys;

    public StartPropertyRealtorSearchAction(string baseUrl, string clientApplication, 
        AsyncPolicy asyncResiliencePolicy, 
        PartnerApiHttpClientFactory httpClientFactory,
        IEnumerable<string> searchKeys) 
        : base(baseUrl, clientApplication, asyncResiliencePolicy, httpClientFactory)
    {
        _searchKeys = searchKeys ?? throw new ArgumentNullException(nameof(searchKeys));
    }

    protected override TimeSpan Timeout => TimeSpan.FromSeconds(10);
    protected override HttpRequestMessage CreateHttpRequest()
    {
        var url = string.Empty
            .SetQueryParam("type", "koop")
            .SetQueryParam("zo", $"/{string.Join('/', _searchKeys)}/")
            .SetQueryParam("page", 1)
            .SetQueryParam("pagesize", MaxPageSize);

        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri(url, UriKind.Relative),
            Method = HttpMethod.Get
        };

        return request;
    }
}
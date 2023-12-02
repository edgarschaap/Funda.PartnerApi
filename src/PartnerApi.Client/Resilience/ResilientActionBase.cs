using System.Reflection;
using PartnerApi.Client.Extensions;
using Polly;

namespace PartnerApi.Client.Resilience;

internal abstract class ResilientActionBase
{
    public string ClientApplication { get; }
    protected string BaseUrl { get; }
        
    protected readonly PartnerApiHttpClientFactory HttpClientFactory;
        
    /// <summary>
    /// Timeout for each client request
    /// </summary>
    protected abstract TimeSpan Timeout { get; }

    protected ResilientActionBase(string baseUrl, string clientApplication, PartnerApiHttpClientFactory httpClientFactory)
    {
        if (string.IsNullOrWhiteSpace(baseUrl))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(baseUrl));
        if (string.IsNullOrWhiteSpace(clientApplication))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(clientApplication));
            
        HttpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));

        BaseUrl = baseUrl;
        ClientApplication = clientApplication;
    }

    protected virtual HttpClient CreateRestClient()
    {
        return HttpClientFactory.Create(BaseUrl);
    }
        
    protected void AddDefaultHeaders(HttpRequestMessage requestMessage, Context context)
    {
        requestMessage.Headers.Add("X-Correlation", Guid.NewGuid().ToString()); // these guids are just fillers, 
        requestMessage.Headers.Add("X-Attempt", Guid.NewGuid().ToString());     // but could be passed it in some way
        requestMessage.Headers.Add("X-HttpCallRetryCount", Convert.ToString(context.GetClientApiContext().RetryCount));
        requestMessage.Headers.Add("ClientApplication", ClientApplication);
    }
}
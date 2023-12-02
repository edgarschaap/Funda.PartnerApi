using Polly;
using Serilog;

namespace PartnerApi.Client.Services;

internal class ClientServiceBase
{
    internal PartnerApiHttpClientFactory HttpClientFactory { get; set; }

    protected readonly string BaseUrl;
    protected readonly string ClientApplication;
    protected readonly AsyncPolicy ResiliencePolicy;
    protected readonly ILogger Logger;
        
    public ClientServiceBase(string baseUrl, string clientApplication,
        AsyncPolicy resiliencePolicy,
        PartnerApiHttpClientFactory httpClientFactory,
        ILogger logger)
    {
        BaseUrl = baseUrl;
        ClientApplication = clientApplication;
        ResiliencePolicy = resiliencePolicy;
        HttpClientFactory = httpClientFactory;
        Logger = logger;
    }
}
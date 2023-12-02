using PartnerApi.Client.Resilience;
using PartnerApi.Client.Services.Search;
using Polly;
using Polly.Registry;
using Serilog;

namespace PartnerApi.Client.Services;

/// <summary>
/// Single factory for the Partner Api Client that creates all instances
/// </summary>
public class ClientPartnerApiServiceFactory
{
    private readonly string _baseUrl;
    private readonly string _clientApplication;
    private readonly ILogger _logger;
    private readonly AsyncPolicy _asyncPolicy;
    private readonly PartnerApiHttpClientFactory _httpClientFactory;
    
    private const string PartnerApiClientPolicyAsync = "PartnerApiClientAsync";
    
    public ClientPartnerApiServiceFactory(string baseUrl, string clientApplication, ILogger logger)
    {
        _baseUrl = baseUrl ?? throw new ArgumentNullException(nameof(baseUrl));
        _clientApplication = clientApplication ?? throw new ArgumentNullException(nameof(clientApplication));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));


        var policies = new PolicyRegistryFactoryDefault(_logger).Create();
        _asyncPolicy = policies.Get<AsyncPolicy>(PartnerApiClientPolicyAsync);
            
        _httpClientFactory = new PartnerApiHttpClientFactory(clientApplication, _baseUrl);
    }

    public IPropertySearchService CreatePropertySearchService()
    {
        return new PropertySearchService(_baseUrl, _clientApplication, _asyncPolicy, _httpClientFactory, _logger);
    }
}
using HttpClientFactoryLite;

namespace PartnerApi.Client;

public sealed class PartnerApiHttpClientFactory
{
    private readonly HttpClientFactory _httpClientFactory;
    private readonly string _httpClientName;
    
    public PartnerApiHttpClientFactory(string applicationName, string baseUrl)
    {
        if (string.IsNullOrWhiteSpace(applicationName)) throw new ArgumentNullException(nameof(applicationName));
        if (string.IsNullOrWhiteSpace(baseUrl)) throw new ArgumentNullException(nameof(baseUrl));
        
        _httpClientName = $"{applicationName}-http-client";
        _httpClientFactory = new HttpClientFactory();
        
        _httpClientFactory.Register(_httpClientName,
            builder =>
            {
                builder
                    .ConfigureHttpClient(baseHttpClient =>
                    {
                        baseHttpClient.BaseAddress = new Uri(baseUrl);
                    });
            });
    }
    
    public HttpClient Create(string url)
    {
        return _httpClientFactory.CreateClient(_httpClientName);
    }
}
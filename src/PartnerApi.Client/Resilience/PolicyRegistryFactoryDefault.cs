using PartnerApi.Client.Exceptions;
using PartnerApi.Client.Extensions;
using Polly;
using Polly.Registry;
using Serilog;

namespace PartnerApi.Client.Resilience;

public class PolicyRegistryFactoryDefault
{
    private readonly ILogger _logger;
    private readonly TimeSpan[] _sleepDurations;
    
    public PolicyRegistryFactoryDefault(ILogger logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _sleepDurations = GetRetrySleepDurations();
    }
    
    public PolicyRegistry Create()
    {
        return new PolicyRegistry()
        {
            {"PartnerApiClientAsync", CreateAsyncRetryPolicy() },
        };
    }
    
    protected virtual void OnRetry(Exception exception, Context context, int retryCount)
    {
        var clientApiPollyContext = context.GetClientApiContext();
        clientApiPollyContext.RetryCount = retryCount;
        
        _logger
            .ForContext("RetryOperation", clientApiPollyContext.Operation)
            .ForContext("RetryCount", retryCount)
            .Warning(exception, "Operation failed. Retry");
    }
    
    protected virtual TimeSpan[] GetRetrySleepDurations()
    {
        return new[]
        {
            TimeSpan.FromSeconds(1),
            TimeSpan.FromSeconds(2),
            TimeSpan.FromSeconds(4),
            TimeSpan.FromSeconds(8)
        };
    }
    
    private AsyncPolicy CreateAsyncRetryPolicy()
    {
        var asyncRetryHttpRequestExceptionPolicy = Policy
            .Handle<RetryTimeoutException>()
            .Or<HttpRequestException>()
            .WaitAndRetryAsync(
                _sleepDurations,
                (delegateResult, sleepDuration, retryCount, context) =>
                {
                    OnRetry(delegateResult, context, retryCount);
                });

        return asyncRetryHttpRequestExceptionPolicy;
    }
}
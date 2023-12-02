using System.Net;
using Newtonsoft.Json;
using PartnerApi.Client.Exceptions;
using PartnerApi.Client.Extensions;
using PartnerApi.Client.Services;
using Polly;

namespace PartnerApi.Client.Resilience;

internal abstract class ResilientActionAsyncBase<TReturn> : ResilientActionBase
{
    private readonly AsyncPolicy _asyncResiliencePolicy;

    protected ResilientActionAsyncBase(string baseUrl, string clientApplication, AsyncPolicy asyncResiliencePolicy,
        PartnerApiHttpClientFactory httpClientFactory) : base(baseUrl, clientApplication, httpClientFactory)
    {
        _asyncResiliencePolicy =
            asyncResiliencePolicy ?? throw new ArgumentNullException(nameof(asyncResiliencePolicy));
    }

    public async Task<IPartnerApiResult<TReturn>> ExecuteAsync(CancellationToken ct)
    {
        try
        {
            HttpResponseMessage response = await ExecuteWithPolicy(_asyncResiliencePolicy, ct);
            return await HandleResult(response, ct);
        }
        catch (JsonSerializationException ex)
        {
            return PartnerApiResult<TReturn>.ClientFailure("Error deserializing response", ex);
        }
        catch (RetryTimeoutException ex)
        {
            return PartnerApiResult<TReturn>.ClientTimeout("Service unavailable: Client timeout", ex);
        }
        catch (PartnerApiServiceUnavailableException ex)
        {
            return PartnerApiResult<TReturn>.FromStatusCode(HttpStatusCode.ServiceUnavailable, ex: ex);
        }
        catch (Exception ex)
        {
            return PartnerApiResult<TReturn>.ClientFailure("Unhandled PartnerApiClient exception", ex);
        }
    }

    private async Task<HttpResponseMessage> ExecuteWithPolicy(
        IAsyncPolicy policy,
        CancellationToken cancellationToken)
    {
        ClientApiPollyContext clientApiPollyContext = CreatePollyContext();
        HttpClient client = CreateRestClient();

        var requestContext = new Context();
        requestContext.AddClientApiContext(clientApiPollyContext);

        return await policy.ExecuteAsync(async (pollyContext, ct) =>
            {
                var restRequest = CreateHttpRequest();

                var httpCallCancellationToken = CancellationTokenSource.CreateLinkedTokenSource(ct);
                httpCallCancellationToken.CancelAfter(Timeout);

                AddDefaultHeaders(restRequest, pollyContext);

                try
                {
                    var httpResponseMessage = await client.SendAsync(restRequest, httpCallCancellationToken.Token);

                    // We only want to retry timeouts as we expect other retries to be happening on the server side
                    if (httpResponseMessage.StatusCode == HttpStatusCode.RequestTimeout)
                    {
                        throw new RetryTimeoutException();
                    }

                    if (httpResponseMessage.StatusCode == HttpStatusCode.ServiceUnavailable)
                    {
                        throw new PartnerApiServiceUnavailableException();
                    }

                    return httpResponseMessage;
                }
                catch (OperationCanceledException)
                {
                    if (httpCallCancellationToken.IsCancellationRequested)
                    {
                        throw new RetryTimeoutException();
                    }

                    throw;
                }
            }, requestContext,
            cancellationToken);
    }

    private ClientApiPollyContext CreatePollyContext()
    {
        return new ClientApiPollyContext(this.GetType().Name);
    }

    /// <summary>
    /// Create the IRestRequest that will be executed using the given policy
    /// </summary>
    /// <returns></returns>
    protected abstract HttpRequestMessage CreateHttpRequest();

    /// <summary>
    /// Handles the HTTP response.<br></br>
    /// If the response is successful, deserializes the output and returns
    /// a <see cref="PartnerApiResult{T}"/>.<see cref="PartnerApiResult{T}.Successful"/> result.<br></br>
    /// 
    /// Otherwise, returns a <see cref="PartnerApiResult{T}"/> based on the response HTTP StatusCode
    /// (see <see cref="PartnerApiResult{T}"/>.<see cref="PartnerApiResult{T}.FromStatusCode"/>)  
    /// </summary>
    /// <param name="response"></param>
    /// <param name="ct"></param>
    /// <returns>IPartnerApiResult</returns>
    protected virtual async Task<IPartnerApiResult<TReturn>> HandleResult(HttpResponseMessage response,
        CancellationToken ct)
    {
        if (response.IsSuccessStatusCode)
        {
            string readAsStringAsync = await response.Content.ReadAsStringAsync();

            // Not all responses contain a body/content
            if (string.IsNullOrWhiteSpace(readAsStringAsync))
            {
                return PartnerApiResult<TReturn>.Successful(default);
            }

            var value = JsonConvert.DeserializeObject<TReturn>(readAsStringAsync);
            return PartnerApiResult<TReturn>.Successful(value);
        }

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            string reason = await response.Content.ReadAsStringAsync();
            return PartnerApiResult<TReturn>.FromStatusCode(response.StatusCode, message: reason);
        }

        return PartnerApiResult<TReturn>.FromStatusCode(response.StatusCode);
    }
}
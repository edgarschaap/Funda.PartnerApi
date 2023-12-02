using PartnerApi.Client.Resilience;
using Polly;

namespace PartnerApi.Client.Extensions;

internal static class PollyExtensions
{
    private const string PollyContextName = "ClientApiPollyContext";

    public static void AddClientApiContext(this Context context, ClientApiPollyContext clientApiPollyContext)
    {
        context.Add(PollyContextName, clientApiPollyContext);
    }
    
    public static ClientApiPollyContext GetClientApiContext(this Context context)
    {
        var clientApiPollyContext = ((ClientApiPollyContext) context[PollyContextName]);
        return clientApiPollyContext;
    }
}
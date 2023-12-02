namespace PartnerApi.Client.Resilience;

internal class ClientApiPollyContext
{
    public ClientApiPollyContext(string operation)
    {
        if (string.IsNullOrWhiteSpace(operation))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(operation));

        Operation = operation;
        RetryCount = 0;
    }

    public string Operation { get; set; }
        
    public int RetryCount { get; set; }
}
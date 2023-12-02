using System.Runtime.Serialization;

namespace PartnerApi.Client.Exceptions;

[Serializable]
internal class RetryTimeoutException : Exception
{
    public RetryTimeoutException() { }

    public RetryTimeoutException(string message) : base(message) { }

    public RetryTimeoutException(string message, Exception inner) : base(message, inner) { }

    protected RetryTimeoutException(
        SerializationInfo info,
        StreamingContext context) : base(info, context) { }
}
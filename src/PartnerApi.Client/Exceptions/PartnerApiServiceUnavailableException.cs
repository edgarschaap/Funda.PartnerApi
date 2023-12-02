using System.Runtime.Serialization;

namespace PartnerApi.Client.Exceptions;

[Serializable]
internal class PartnerApiServiceUnavailableException : Exception
{
    public PartnerApiServiceUnavailableException() { }

    public PartnerApiServiceUnavailableException(string message) : base(message) { }
    
    public PartnerApiServiceUnavailableException(string message, Exception inner) : base(message, inner) { }

    protected PartnerApiServiceUnavailableException(
        SerializationInfo info,
        StreamingContext context) : base(info, context) { }
}
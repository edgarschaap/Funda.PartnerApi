using System.Runtime.Serialization;

namespace Host.Console.Exceptions;

[Serializable]
public class InvalidApplicationConfigurationException : Exception
{
    public InvalidApplicationConfigurationException()
    {
    }

    public InvalidApplicationConfigurationException(string message) : base(message)
    {
    }

    public InvalidApplicationConfigurationException(string message, Exception inner) : base(message, inner)
    {
    }

    protected InvalidApplicationConfigurationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
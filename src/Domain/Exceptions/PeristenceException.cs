using System.Runtime.Serialization;

namespace Domain.Exceptions;

public class PersistenceException : Exception
{
    private const string DefaultMessage = "An exception occurred during a persistence call";

    public PersistenceException(bool isTransient) : base(DefaultMessage)
    {
        IsTransient = isTransient;
    }

    public PersistenceException(bool isTransient, string message) : base(message)
    {
        IsTransient = isTransient;
    }

    public PersistenceException(bool isTransient, Exception inner) : base(DefaultMessage, inner)
    {
        IsTransient = isTransient;
    }

    public PersistenceException(bool isTransient, string message, Exception inner) : base(message, inner)
    {
        IsTransient = isTransient;
    }

    protected PersistenceException(
        SerializationInfo info,
        StreamingContext context) : base(info, context)
    {
    }

    public bool IsTransient { get; private set; }
}
namespace PartnerApi.Client.Services;

public interface IPartnerApiResult<TReturn>
{
    bool IsSuccessful { get; }
    bool IsFailure { get; }
    TReturn Value { get; }
    ResponseStatus Status { get; }

    /// <summary>
    /// If the operation wasn't successful, a message indicating why it was not successful.
    /// </summary>
    string Message { get; }

    /// <summary>
    /// If Success is false and an exception has been caught internally, this field will contain the exception.
    /// </summary>
    Exception Exception { get; }
}
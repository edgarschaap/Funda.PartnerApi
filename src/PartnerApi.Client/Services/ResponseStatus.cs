namespace PartnerApi.Client.Services;

public enum ResponseStatus
{
    BadRequest = -6, // Inidicates there is an error with the supplied call parameters and it should not be retried
    ServiceUnavailable = -5,  // Indicates a transient error, eg the Partner Api use all retries and still failed
    Timeout = -4,  // Indicates not able to establish a connection to Partner Export} Api
    Failure = -3,  // General failure, 5xx etc...
    Success = 0,
    NotFound = 1,  // Successful request but the resource does not exist
    Unauthorized = 2
}
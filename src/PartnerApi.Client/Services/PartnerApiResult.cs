using System.Net;

namespace PartnerApi.Client.Services;

public class PartnerApiResult<TReturn> : IPartnerApiResult<TReturn>
{
    public bool IsSuccessful
    {
        get
        {
            _successChecked = true;
            return _isSuccessful;
        }
        private set { _isSuccessful = value; }
    }

    private bool _isSuccessful;
    private bool _successChecked = false;

    public bool IsFailure => !IsSuccessful;

    /// <summary>
    /// Indicates the condition that caused this call to fail is transient and the client OR service
    /// has used all of its retries and still failed
    /// </summary>
    public bool IsTransient { get; private set; }

    public TReturn Value
    {
        get
        {
            if (!_successChecked)
            {
                throw new InvalidOperationException("Not permitted to access value before checking for success");
            }

            return _value;
        }

        private set { _value = value; }
    }

    private TReturn _value;

    public ResponseStatus Status { get; }
    public string? Message { get; private init; }
    public Exception? Exception { get; private set; }

    private PartnerApiResult(bool success, ResponseStatus responseStatus, bool isTransient, TReturn value = default)
    {
        IsSuccessful = success;
        Status = responseStatus;
        IsTransient = isTransient;
        Value = value;
    }

    public static PartnerApiResult<TReturn> Successful(TReturn value)
    {
        var partnerApiResult = new PartnerApiResult<TReturn>(true, ResponseStatus.Success, false, value);
        return partnerApiResult;
    }

    public static PartnerApiResult<TReturn> ClientTimeout(string message = null, Exception ex = null)
    {
        var partnerApiResult = new PartnerApiResult<TReturn>(false, ResponseStatus.Timeout, true)
        {
            Message = message,
            Exception = ex
        };

        return partnerApiResult;
    }

    public static PartnerApiResult<TReturn> ClientFailure(string message = null, Exception ex = null)
    {
        var partnerApiResult = new PartnerApiResult<TReturn>(false, ResponseStatus.Failure, false)
        {
            Message = message,
            Exception = ex
        };

        return partnerApiResult;
    }

    /// <summary>
    /// Create a PartnerApiResult<TResult> based on value of HttpStatusCode
    /// 
    /// </summary>
    /// <param name="httpStatusCode"></param>
    /// <param name="value"></param>
    /// <param name="message"></param>
    /// <param name="ex"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static PartnerApiResult<TReturn> FromStatusCode(HttpStatusCode httpStatusCode, TReturn value = default,
        string message = null,
        Exception ex = null)
    {
        ResponseStatus responseStatus = ResponseStatus.Failure;
        bool isSuccessful = false;
        bool isTransient = false;

        if ((int)httpStatusCode < 400)
        {
            isSuccessful = true;
            responseStatus = ResponseStatus.Success;
        }
        else if (httpStatusCode == HttpStatusCode.NotFound)
        {
            isSuccessful = true;
            responseStatus = ResponseStatus.NotFound;
        }
        else if (httpStatusCode == HttpStatusCode.ServiceUnavailable)
        {
            responseStatus = ResponseStatus.ServiceUnavailable;
            isTransient = true;
        }
        else if (httpStatusCode == HttpStatusCode.BadRequest)
        {
            responseStatus = ResponseStatus.BadRequest;
        }

        var partnerApiResult = new PartnerApiResult<TReturn>(isSuccessful, responseStatus, isTransient)
        {
            Message = message,
            Value = value
        };

        if (!isSuccessful)
        {
            partnerApiResult.Exception = ex;
        }

        return partnerApiResult;
    }
}
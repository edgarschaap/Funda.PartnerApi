namespace Host.Console.Configuration;

public interface ISettings
{
    string PartnerApiBaseUri { get; }
    string PartnerApiAuthToken { get; }
}
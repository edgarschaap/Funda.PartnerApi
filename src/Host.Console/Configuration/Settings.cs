namespace Host.Console.Configuration;

public class Settings : ISettings
{
    public string PartnerApiBaseUri { get; set; }
    public string PartnerApiAuthToken { get; set; }
}
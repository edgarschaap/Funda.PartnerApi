using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Yaml;

namespace Host.Console.Configuration;

public class ApplicationConfiguration
{
    public IConfigurationBuilder CreateBuilder()
    {
        var configurationBuilder = new ConfigurationBuilder();

        configurationBuilder
            .AddYamlFile("settings.yaml", true)
            .AddEnvironmentVariables();

        return configurationBuilder;
    }
}
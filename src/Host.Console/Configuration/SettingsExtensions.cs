using Microsoft.Extensions.Configuration;

namespace Host.Console.Configuration;

public static class SettingsExtensions
{
    public static TSettings ToSettings<TSettings>(this IConfiguration configuration, string applicationName)
    {
        var settings = Activator.CreateInstance<TSettings>();

        // override environment wide settings with application specific settings
        settings
            .BindEnvironmentWideSettings(configuration)
            .BindApplicationSpecificSettings(configuration, applicationName);

        return settings;
    }
    
    private static TSettings BindApplicationSpecificSettings<TSettings>(this TSettings settings, IConfiguration configuration,
        string applicationName)
    {
        var applicationSpecificSection = configuration.GetSection(applicationName);
        applicationSpecificSection.Bind(settings);
        return settings;
    }

    private static TSettings BindEnvironmentWideSettings<TSettings>(this TSettings settings, IConfiguration configuration)
    {
        configuration.Bind(settings);
        return settings;
    }
}
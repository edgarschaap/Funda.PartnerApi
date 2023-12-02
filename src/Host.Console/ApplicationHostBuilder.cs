using Adapter.Http.PartnerApi;
using Domain.Ports;
using Domain.UseCases;
using Flurl;
using Host.Console.Configuration;
using Host.Console.Exceptions;
using Host.Console.Services;
using Microsoft.Extensions.Hosting;
using PartnerApi.Client.Services;
using Serilog;
using SimpleInjector;

namespace Host.Console;

public class ApplicationHostBuilder
{
    private readonly string[] _args;
    private readonly Container _container;
    private readonly ISettings _settings;
    private readonly string _applicationName;
    private readonly ILogger _logger;

    public ApplicationHostBuilder(string[] args, Container container, ISettings settings, string applicationName, ILogger logger)
    {
        _args = args ?? throw new ArgumentNullException(nameof(args));
        _container = container ?? throw new ArgumentNullException(nameof(container));
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        _applicationName = applicationName ?? throw new ArgumentNullException(nameof(applicationName));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public virtual IHost Build()
    {
        var host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(_args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSimpleInjector(_container, options =>
                {
                    options.AddHostedService<ConsoleService>();
                });
            })
            .UseConsoleLifetime()
            .Build()
            .UseSimpleInjector(_container);

        PerformPreflightChecks();
        RegisterDependencies();

        return host;
    }

    private void RegisterDependencies()
    {
        _container.RegisterInstance(_settings);
        _container.RegisterInstance(_logger);

        var baseUrl = _settings.PartnerApiBaseUri
            .AppendPathSegment(_settings.PartnerApiAuthToken)
            .AppendPathSegment("/");

        var partnerApiServiceFactory = new ClientPartnerApiServiceFactory(baseUrl, _applicationName, _logger);
        var propertySearchService = partnerApiServiceFactory.CreatePropertySearchService();
        
        _container.RegisterInstance(propertySearchService);

        IRealtorRepository realtorRepository = new PartnerApiRealtorRepository(propertySearchService);
        
        _container.RegisterInstance(realtorRepository);
        _container.RegisterSingleton<GetTopRealtorsWithPropertiesForSearchKeyUseCase>();
    }

    private void PerformPreflightChecks()
    {
        if (string.IsNullOrWhiteSpace(_settings.PartnerApiBaseUri))
        {
            throw new InvalidApplicationConfigurationException("Required PartnerApi baseUrl configuration missing");
        }

        if (string.IsNullOrWhiteSpace(_settings.PartnerApiAuthToken) ||
            string.Equals(_settings.PartnerApiAuthToken, "<secret not set>", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidApplicationConfigurationException("Required PartnerApi auth token configuration not set");
        }
        
        // Usually i would also do a check for different external dependencies on like a gtg endpoint, to ensure they are available
    }
}
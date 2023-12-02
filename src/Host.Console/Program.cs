using Host.Console.Configuration;
using Host.Console.Configuration.Logging;
using Microsoft.Extensions.Hosting;
using Serilog;
using SimpleInjector;

namespace Host.Console;

public class Program
{
    private const string ApplicationName = "ParterApiAssessment";

    static void Main(string[] args)
    {
        var applicationConfiguration = new ApplicationConfiguration().CreateBuilder().Build();

        var logger = SerilogConfiguration.Create(ApplicationName);
        logger.Information("Application Started..");
        
        ISettings settings = applicationConfiguration.ToSettings<Settings>(ApplicationName);

        var container = new Container();

        using (var host = new ApplicationHostBuilder(args, container, settings, ApplicationName, logger).Build())
        {
            host.Start();
            host.WaitForShutdown();
            
            Log.Information("Shutting down ...");
            Log.CloseAndFlush();
        }
    }
}
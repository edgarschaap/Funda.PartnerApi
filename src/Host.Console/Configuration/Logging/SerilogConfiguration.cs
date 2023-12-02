using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Host.Console.Configuration.Logging;

public class SerilogConfiguration
{
    public static ILogger Create(string applicationName)
    {
        var configuration = new LoggerConfiguration()
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message,-30:lj} {Properties:j}{NewLine}{Exception}",
                theme: AnsiConsoleTheme.Literate
            );

        var logger = configuration.CreateLogger();

        Log.Logger = logger;

        return logger;
    }
}
using ConsoleTables;
using Domain.UseCases;
using Domain.ValueTypes;
using Host.Console.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Host.Console.Services;

public class ConsoleService : BackgroundService
{
    private readonly ISettings _settings;
    private readonly IHostApplicationLifetime _applicationLifetime;
    private readonly ILogger _logger;
    private readonly GetTopRealtorsWithPropertiesForSearchKeyUseCase _getTopRealtorsWithPropertiesForSearchKeyUseCase;

    public ConsoleService(ISettings settings,
        IHostApplicationLifetime applicationLifetime,
        ILogger logger,
        
        GetTopRealtorsWithPropertiesForSearchKeyUseCase getTopRealtorsWithPropertiesForSearchKeyUseCase)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        _applicationLifetime = applicationLifetime ?? throw new ArgumentNullException(nameof(applicationLifetime));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _getTopRealtorsWithPropertiesForSearchKeyUseCase = getTopRealtorsWithPropertiesForSearchKeyUseCase ?? throw new ArgumentNullException(nameof(getTopRealtorsWithPropertiesForSearchKeyUseCase));
    }


    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var amsterdamSearchKey = SearchKey.New("Amsterdam");
        var tuinSearchKey = SearchKey.New("Tuin");

        while (true)
        {
            var keys = new List<SearchKey>();
        
            System.Console.WriteLine("Please select search keys to find realtors with most properties ...");
            System.Console.WriteLine(" 1. Amsterdam");
            System.Console.WriteLine(" 2. Amsterdam | Tuin");
            System.Console.WriteLine("-------------------------------------------------------------------");

            ConsoleKeyInfo keyRead = System.Console.ReadKey();
            
            switch (keyRead.Key)
            {
                case ConsoleKey.D1:
                    keys.Add(amsterdamSearchKey);
                    break;
            
                case ConsoleKey.D2:
                    keys.Add(amsterdamSearchKey);
                    keys.Add(tuinSearchKey);
                    break;
            }
        
            var result = await _getTopRealtorsWithPropertiesForSearchKeyUseCase.ExecuteAsync(keys, cancellationToken);

            System.Console.WriteLine("");
            System.Console.WriteLine($"These are the top 10 realtors with most properties for keywords: {string.Join('|', keys)}");
            
            var table = new ConsoleTable("RealtorName", "PropertyCount");
            
            foreach (var realtorProperty in result)
            {
                table.AddRow(realtorProperty.RealtorName.Value, realtorProperty.PropertyCount.Value);
            }
            
            table.Write();
        }
    }
}
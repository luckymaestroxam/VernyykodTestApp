using CurrencyParserService.ConsoleApp.Startup;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .UseContentRoot(AppContext.BaseDirectory)
    .ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration;
        services.AddCbrHttpClient(configuration);
        services.AddServices();
        services.AddRepositories(configuration);
        services.AddCurrencyParserBackgroundService(configuration);
    })
    .Build();

await host.RunAsync();

using ConsoleApp.Startup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        services.AddCbrHttpClient(configuration);
        services.AddServices();
        services.AddCurrencyParserBackgroundService(configuration);
    })
    .Build();

await host.RunAsync();

using ConsoleApp.Startup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

await using var serviceProvider = new ServiceCollection()
    .AddMigrations(configuration)
    .BuildServiceProvider();
await serviceProvider.MigrateDatabase(configuration);

Console.WriteLine("Миграции успешно выполнены.");

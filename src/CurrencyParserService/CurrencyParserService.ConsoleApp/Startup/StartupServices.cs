using CurrencyParserService.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using ParserService = CurrencyParserService.Application.Services.CurrencyParserService;

namespace CurrencyParserService.ConsoleApp.Startup;

public static class StartupServices
{
    public static void AddServices(this IServiceCollection services) =>
        services.AddScoped<ICurrencyParserService, ParserService>();
}

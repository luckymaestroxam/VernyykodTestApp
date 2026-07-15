using Application.Interfaces;
using Infrastructure.HttpClients;
using Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp.Startup;

public static class StartupCbrHttpClient
{
    public static IServiceCollection AddCbrHttpClient(this IServiceCollection services, IConfiguration configuration)
    {
        var baseUrl = configuration["cbr:http:baseUrl"];
        var xmlDailyUrl = configuration["cbr:http:xmldailyurl"];

        var options = new CbrOptions(new Uri(new Uri(baseUrl), xmlDailyUrl).ToString());
        services.AddSingleton(options);
        services.AddHttpClient<IDailyRateProvider, CbrHttpClient>();

        return services;
    }
}

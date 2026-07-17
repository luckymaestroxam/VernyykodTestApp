using Application.Interfaces;
using Application.RequestHandlers.AddUserCurrency;
using Application.RequestHandlers.GetUserCurrencies;

namespace Api.Startup;

public static class StartupServices
{
    public static void AddServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IRequestHandler<GetUserCurrenciesRequest, GetUserCurrenciesResponse>,
            GetUserCurrenciesRequestHandler>();
        builder.Services.AddScoped<IRequestHandler<AddUserCurrencyRequest, AddUserCurrencyResponse>,
            AddUserCurrencyRequestHandler>();
    }
}

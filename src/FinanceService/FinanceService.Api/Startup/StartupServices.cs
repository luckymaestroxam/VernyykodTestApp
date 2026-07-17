using FinanceService.Application.RequestHandlers.AddUserCurrency;
using FinanceService.Application.RequestHandlers.GetUserCurrencies;
using FinanceService.Application.RequestHandlers.RemoveUserCurrency;
using Shared.Application.Interfaces;

namespace FinanceService.Api.Startup;

public static class StartupServices
{
    public static void AddServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IRequestHandler<GetUserCurrenciesRequest, GetUserCurrenciesResponse>,
            GetUserCurrenciesRequestHandler>();
        builder.Services.AddScoped<IRequestHandler<AddUserCurrencyRequest, AddUserCurrencyResponse>,
            AddUserCurrencyRequestHandler>();
        builder.Services.AddScoped<IRequestHandler<RemoveUserCurrencyRequest, RemoveUserCurrencyResponse>,
            RemoveUserCurrencyRequestHandler>();
    }
}

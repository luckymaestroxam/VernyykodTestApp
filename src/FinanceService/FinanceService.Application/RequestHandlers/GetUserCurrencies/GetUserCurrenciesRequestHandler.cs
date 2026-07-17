using FinanceService.Application.Interfaces;
using Shared.Application.Interfaces;

namespace FinanceService.Application.RequestHandlers.GetUserCurrencies;

public class GetUserCurrenciesRequestHandler(IUserCurrencyReadRepository userCurrencyReadRepository)
    : IRequestHandler<GetUserCurrenciesRequest, GetUserCurrenciesResponse>
{
    public async Task<GetUserCurrenciesResponse>
        Handle(GetUserCurrenciesRequest request, CancellationToken cancellationToken)
    {
        var userCurrencies = await userCurrencyReadRepository.GetMany(request.UserId, cancellationToken);

        return new GetUserCurrenciesResponse(userCurrencies);
    }
}

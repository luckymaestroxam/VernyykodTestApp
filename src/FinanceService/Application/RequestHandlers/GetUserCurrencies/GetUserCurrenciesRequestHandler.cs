using Application.Interfaces;

namespace Application.RequestHandlers.GetUserCurrencies;

public class GetUserCurrenciesRequestHandler : IRequestHandler<GetUserCurrenciesRequest, GetUserCurrenciesResponse>
{
    public async Task<GetUserCurrenciesResponse>
        Handle(GetUserCurrenciesRequest request, CancellationToken cancellationToken)
    {
        return new GetUserCurrenciesResponse(null);
    }
}

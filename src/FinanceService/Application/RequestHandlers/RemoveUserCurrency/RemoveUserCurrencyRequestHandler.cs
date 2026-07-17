using Application.Exceptions;
using Application.Interfaces;
using Domain.Aggregates;
using Domain.ValueObjects;

namespace Application.RequestHandlers.RemoveUserCurrency;

public class RemoveUserCurrencyRequestHandler(
    ICurrencyReadRepository currencyReadRepository,
    IUserCurrencyWriteRepository userCurrencyWriteRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<RemoveUserCurrencyRequest, RemoveUserCurrencyResponse>
{
    public async Task<RemoveUserCurrencyResponse> Handle(RemoveUserCurrencyRequest request, CancellationToken ct)
    {
        var currencyId = CurrencyId.Create(request.CurrencyId);
        if (!await currencyReadRepository.Exists(currencyId, ct))
        {
            throw new CurrencyNotExistsException("Валюта не найдена.");
        }

        var userCurrency = UserCurrency.Create(request.UserId, currencyId);
        await userCurrencyWriteRepository.Remove(userCurrency, ct);
        await unitOfWork.SaveChanges(ct);

        return new RemoveUserCurrencyResponse();
    }
}

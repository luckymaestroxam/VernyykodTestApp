using Application.Exceptions;
using Application.Interfaces;
using Domain.Aggregates;
using Domain.ValueObjects;

namespace Application.RequestHandlers.AddUserCurrency;

public class AddUserCurrencyRequestHandler(
    ICurrencyReadRepository currencyReadRepository,
    IUserCurrencyWriteRepository userCurrencyWriteRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<AddUserCurrencyRequest, AddUserCurrencyResponse>
{
    public async Task<AddUserCurrencyResponse> Handle(AddUserCurrencyRequest request, CancellationToken ct)
    {
        var currencyId = CurrencyId.Create(request.CurrencyId);
        if (!await currencyReadRepository.Exists(currencyId, ct))
        {
            throw new CurrencyNotExistsException("Валюта не найдена.");
        }

        var userCurrency = UserCurrency.Create(request.UserId, currencyId);
        try
        {
            await userCurrencyWriteRepository.Add(userCurrency, ct);
            await unitOfWork.SaveChanges(ct);
        }
        catch (RepositoryConflictException ex)
        {
            throw new UserCurrencyAlreadyAddedException("Избранная валюта уже добавлена.", ex);
        }

        return new AddUserCurrencyResponse();
    }
}

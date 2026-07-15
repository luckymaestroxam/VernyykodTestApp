using Domain.Aggregates;

namespace Application.Interfaces;

public interface IDailyRateProvider
{
    Task<Currency[]> GetDailyRates(CancellationToken cancellationToken = default);
}

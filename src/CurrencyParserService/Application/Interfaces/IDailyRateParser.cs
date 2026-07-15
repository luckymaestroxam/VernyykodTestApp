using Domain.Aggregates;

namespace Application.Interfaces;

public interface IDailyRateParser
{
    Task<Currency[]> GetDailyRates(CancellationToken cancellationToken = default);
}

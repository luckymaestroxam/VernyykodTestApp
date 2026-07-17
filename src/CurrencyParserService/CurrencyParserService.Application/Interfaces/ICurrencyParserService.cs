namespace CurrencyParserService.Application.Interfaces;

public interface ICurrencyParserService
{
    Task Parse(CancellationToken stoppingToken);
}

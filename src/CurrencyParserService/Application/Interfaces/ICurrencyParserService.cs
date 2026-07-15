namespace Application.Interfaces;

public interface ICurrencyParserService
{
    Task Parse(CancellationToken stoppingToken);
}

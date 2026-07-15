namespace Infrastructure.Dtos;

public class CurrencyDto
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public decimal Rate { get; set; }
}

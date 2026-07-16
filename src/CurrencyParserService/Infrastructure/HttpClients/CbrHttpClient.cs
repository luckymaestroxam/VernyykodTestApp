using System.Globalization;
using System.Text;
using System.Xml.Linq;
using Application.Interfaces;
using Domain.Aggregates;
using Domain.ValueObjects;
using Infrastructure.Options;

namespace Infrastructure.HttpClients;

public class CbrHttpClient(HttpClient httpClient, CbrOptions cbrOptions) : IDailyRateProvider
{
    private static readonly CultureInfo RussianCulture = CultureInfo.GetCultureInfo("ru-RU");
    private static readonly Encoding Windows1251Encoding;

    static CbrHttpClient()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        Windows1251Encoding = Encoding.GetEncoding(1251);
    }

    public async Task<Currency[]> GetDailyRates(CancellationToken cancellationToken)
    {
        using var response = await httpClient.GetAsync(cbrOptions.XmlDailyFullUrl, cancellationToken);
        response.EnsureSuccessStatusCode();

        var bytes = await response.Content.ReadAsByteArrayAsync(cancellationToken);
        var xml = Windows1251Encoding.GetString(bytes);
        var document = XDocument.Parse(xml);

        return document.Root?.Elements("Valute").Select(ParseValute).ToArray() ?? [];
    }

    private static Currency ParseValute(XElement valute)
    {
        var id = (string?)valute.Attribute("ID");
        var name = valute.Element("Name")?.Value;
        var rate = ParseUnitRate(valute);

        return Currency.Create(CurrencyId.Create(id), CurrencyName.Create(name), CurrencyRate.Create(rate));
    }

    private static decimal ParseUnitRate(XElement valute)
    {
        if (TryParseDecimal(valute.Element("VunitRate")?.Value, out var unitRate))
        {
            return unitRate;
        }

        if (!TryParseDecimal(valute.Element("Value")?.Value, out var value) ||
            !int.TryParse(valute.Element("Nominal")?.Value, NumberStyles.Integer, RussianCulture, out var nominal) ||
            nominal <= 0)
        {
            return 0m;
        }

        return value / nominal;
    }

    private static bool TryParseDecimal(string? value, out decimal result) =>
        decimal.TryParse(value, NumberStyles.Number, RussianCulture, out result);
}

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

    public async Task<Currency[]> GetDailyRates(CancellationToken cancellationToken = default)
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
        decimal.TryParse(valute.Element("Value")?.Value, NumberStyles.Number, RussianCulture, out var rate);

        return Currency.Create(CurrencyId.Create(id), CurrencyName.Create(name), CurrencyRate.Create(rate));
    }
}

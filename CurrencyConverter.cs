using System.Text.Json;

namespace RadioButtons;

public class CurrencyConverter
{
    private HttpClient client = new();
    private CurrencyRatesStorage storage = new();

    private Dictionary<string, CurrencyRates> rates = new();

    private string url = "https://open.er-api.com/v6/latest";

    public async Task LoadRates()
    {
        string[] currencies = { "USD", "BRL", "EUR" };

        foreach (string currency in currencies)
        {
            try
            {
                string json = await client.GetStringAsync($"{url}/{currency}");

                CurrencyRates? ratesData = JsonSerializer.Deserialize<CurrencyRates>(json);

                if (ratesData != null && ratesData.rates.Count > 0)
                {
                    rates[currency] = ratesData;
                    await storage.Save(currency, ratesData);
                }
            }
            catch
            {
                CurrencyRates? ratesData = await storage.Load(currency);

                if (ratesData != null)
                    rates[currency] = ratesData;
            }
        }
    }

    private List<string> currencies = new() { "BRL", "USD", "EUR" };

    public List<string> GetCurrencies()
    {
        return currencies;
    }

    public decimal ConvertValue(decimal value, string from, string to)
    {
        if (!rates.ContainsKey("USD"))
            return 0;

        var cotacoes = rates["USD"].rates;

        if (!cotacoes.ContainsKey(from) || !cotacoes.ContainsKey(to))
            return 0;

        return value * cotacoes[to] / cotacoes[from];
    }
}

using System.Text.Json;

namespace RadioButtons;

public class CurrencyRatesStorage
{
    public async Task Save(string currency, CurrencyRates rates)
    {
        string path = Path.Combine(FileSystem.AppDataDirectory, $"{currency}.json");
        string json = JsonSerializer.Serialize(rates);

        await File.WriteAllTextAsync(path, json);
    }

    public async Task<CurrencyRates?> Load(string currency)
    {
        string path = Path.Combine(FileSystem.AppDataDirectory, $"{currency}.json");
        if (!File.Exists(path))
            return null;

        string json = await File.ReadAllTextAsync(path);
        return JsonSerializer.Deserialize<CurrencyRates>(json);
    }
}

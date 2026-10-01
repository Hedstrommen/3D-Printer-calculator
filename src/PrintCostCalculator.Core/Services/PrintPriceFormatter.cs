using System.Globalization;

namespace PrintCostCalculator.Core.Services;

/// <summary>
/// Formats money amounts for the currency the user picked.
/// Registered in DI so the web UI and a future MAUI app share the same formatting.
/// </summary>
public class PrintPriceFormatter
{
    private static readonly Dictionary<string, CultureInfo> CurrencyCultures = new()
    {
        ["SEK"] = new CultureInfo("sv-SE"),
        ["EUR"] = new CultureInfo("de-DE"),
        ["USD"] = new CultureInfo("en-US")
    };

    /// <summary>Formats an amount like "149,40 kr" for SEK, "149,40 €" for EUR, "$149.40" for USD.</summary>
    public string Format(decimal amount, string currencyCode)
    {
        if (!CurrencyCultures.TryGetValue(currencyCode, out var culture))
        {
            culture = CultureInfo.InvariantCulture;
        }

        return amount.ToString("N2", culture) + " " + currencyCode;
    }
}

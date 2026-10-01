using PrintCostCalculator.Core.Models;

namespace PrintCostCalculator.Web.Services;

/// <summary>
/// Saves and loads the user's last inputs (filament price, weight, multiplier...)
/// so they don't have to re-type them on the next visit.
/// Implementations decide *where* to store it (browser local storage, database, ...).
/// </summary>
public interface IUserInputStore
{
    Task<SavedPrintCostInput?> LoadAsync();
    Task SaveAsync(PrintCostInput input, string currencyCode);
}

/// <summary>
/// What we remember between visits.
/// </summary>
public record SavedPrintCostInput(
    decimal FilamentPurchasePrice,
    decimal FilamentSpoolWeightInGrams,
    decimal SalesMultiplier,
    decimal ExtraFixedFeePerPrint,
    decimal CostPerPrinterHour,
    string CurrencyCode);

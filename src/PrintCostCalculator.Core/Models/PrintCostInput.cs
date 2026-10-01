namespace PrintCostCalculator.Core.Models;

/// <summary>
/// Everything the user types into the calculator.
/// All money values are in the currency the user selected (e.g. SEK).
/// </summary>
public class PrintCostInput
{
    /// <summary>What the filament spool cost to purchase (e.g. 249 kr).</summary>
    public decimal FilamentPurchasePrice { get; set; }

    /// <summary>Total weight of filament on the spool, in grams (e.g. 1000 g).</summary>
    public decimal FilamentSpoolWeightInGrams { get; set; }

    /// <summary>How many grams of filament the model will use (e.g. 55 g).</summary>
    public decimal ModelWeightInGrams { get; set; }

    /// <summary>Personal sales multiplier. 3.0 means "triple the cost".</summary>
    public decimal SalesMultiplier { get; set; } = 3.0m;

    /// <summary>Optional extra fixed fee per print, e.g. machine wear (0 by default).</summary>
    public decimal ExtraFixedFeePerPrint { get; set; }

    /// <summary>Optional cost per printer hour (electricity, wear), 0 by default.</summary>
    public decimal CostPerPrinterHour { get; set; }

    /// <summary>Optional print duration in hours (only matters if CostPerPrinterHour > 0).</summary>
    public decimal PrintDurationInHours { get; set; }
}

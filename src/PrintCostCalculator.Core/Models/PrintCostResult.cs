namespace PrintCostCalculator.Core.Models;

/// <summary>
/// The full price breakdown shown to the user.
/// </summary>
public class PrintCostResult
{
    /// <summary>Cost of the filament actually used by the model.</summary>
    public decimal MaterialCost { get; set; }

    /// <summary>Optional fixed fee (machine wear etc.), included as-is.</summary>
    public decimal FixedFeeCost { get; set; }

    /// <summary>Optional time-based cost (hours x cost per hour).</summary>
    public decimal TimeCost { get; set; }

    /// <summary>Material + fixed fee + time. The "what it actually cost" number.</summary>
    public decimal TotalCost => MaterialCost + FixedFeeCost + TimeCost;

    /// <summary>TotalCost multiplied by the personal sales multiplier.</summary>
    public decimal SellingPrice => TotalCost * SalesMultiplier;

    /// <summary>The multiplier used, kept so the UI can show it.</summary>
    public decimal SalesMultiplier { get; set; }
}

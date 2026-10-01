using PrintCostCalculator.Core.Models;

namespace PrintCostCalculator.Core.Services;

/// <summary>
/// Default implementation of the price calculation.
/// Swap this out in DI (Program.cs) if you want a different pricing model.
/// </summary>
public class PrintCostCalculatorService : IPrintCostCalculator
{
    public PrintCostResult CalculateCost(PrintCostInput userInput)
    {
        // Price per gram: e.g. 249 kr / 1000 g = 0.249 kr per gram.
        decimal pricePerGram = userInput.FilamentSpoolWeightInGrams == 0
            ? 0
            : userInput.FilamentPurchasePrice / userInput.FilamentSpoolWeightInGrams;

        decimal materialCost = pricePerGram * userInput.ModelWeightInGrams;
        decimal fixedFeeCost = userInput.ExtraFixedFeePerPrint;
        decimal timeCost = userInput.CostPerPrinterHour * userInput.PrintDurationInHours;

        return new PrintCostResult
        {
            MaterialCost = materialCost,
            FixedFeeCost = fixedFeeCost,
            TimeCost = timeCost,
            SalesMultiplier = userInput.SalesMultiplier
        };
    }
}

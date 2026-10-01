using PrintCostCalculator.Core.Models;

namespace PrintCostCalculator.Core.Services;

/// <summary>
/// The single service responsible for turning user input into a price breakdown.
/// Register this in dependency injection (see PrintCostCalculator.Web Program.cs).
/// </summary>
public interface IPrintCostCalculator
{
    PrintCostResult CalculateCost(PrintCostInput userInput);
}

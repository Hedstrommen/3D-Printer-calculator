using PrintCostCalculator.Core.Models;
using PrintCostCalculator.Core.Services;
using Xunit;

namespace PrintCostCalculator.Core.Tests;

public class PrintCostCalculatorServiceTests
{
    private readonly PrintCostCalculatorService calculator = new();

    [Fact]
    public void MaterialCost_IsPricePerGramTimesModelWeight()
    {
        // 249 kr for 1000 g => 0.249 kr/g. Model uses 55 g => 13.695 kr.
        var result = calculator.CalculateCost(new PrintCostInput
        {
            FilamentPurchasePrice = 249,
            FilamentSpoolWeightInGrams = 1000,
            ModelWeightInGrams = 55
        });

        Assert.Equal(13.695m, result.MaterialCost, 3);
    }

    [Fact]
    public void SellingPrice_IsTotalCostTimesMultiplier()
    {
        // Cost 100 kr with multiplier 3 => 300 kr selling price.
        var result = calculator.CalculateCost(new PrintCostInput
        {
            FilamentPurchasePrice = 100,
            FilamentSpoolWeightInGrams = 1000,
            ModelWeightInGrams = 1000,
            SalesMultiplier = 3.0m
        });

        Assert.Equal(100m, result.TotalCost);
        Assert.Equal(300m, result.SellingPrice);
    }

    [Fact]
    public void TimeCost_IsHoursTimesCostPerHour()
    {
        var result = calculator.CalculateCost(new PrintCostInput
        {
            FilamentPurchasePrice = 100,
            FilamentSpoolWeightInGrams = 1000,
            ModelWeightInGrams = 100,
            CostPerPrinterHour = 5,
            PrintDurationInHours = 4
        });

        Assert.Equal(20m, result.TimeCost);
        Assert.Equal(30m, result.TotalCost); // 10 material + 20 time
    }

    [Fact]
    public void EmptySpoolWeight_DoesNotCrash()
    {
        var result = calculator.CalculateCost(new PrintCostInput());

        Assert.Equal(0m, result.MaterialCost);
        Assert.Equal(0m, result.TotalCost);
        Assert.Equal(0m, result.SellingPrice);
    }
}

using Microsoft.JSInterop;
using PrintCostCalculator.Core.Models;
using System.Text.Json;

namespace PrintCostCalculator.Web.Services;

/// <summary>
/// Stores the last inputs in the browser's local storage.
/// Swap this class in DI for a different storage (e.g. a database) if needed.
/// </summary>
public class BrowserUserInputStore : IUserInputStore
{
    private const string StorageKey = "printCostCalculatorLastInputs";

    private readonly IJSRuntime _browserJavascript;

    public BrowserUserInputStore(IJSRuntime browserJavascript)
    {
        _browserJavascript = browserJavascript;
    }

    public async Task<SavedPrintCostInput?> LoadAsync()
    {
        try
        {
            var storedJson = await _browserJavascript.InvokeAsync<string>(
                "localStorage.getItem", StorageKey);

            if (string.IsNullOrEmpty(storedJson))
            {
                return null;
            }

            return JsonSerializer.Deserialize<SavedPrintCostInput>(storedJson);
        }
        catch (JSDisconnectedException)
        {
            // Blazor circuit not ready yet (e.g. prerendering) — nothing saved.
            return null;
        }
    }

    public async Task SaveAsync(PrintCostInput input, string currencyCode)
    {
        var dataToStore = new SavedPrintCostInput(
            input.FilamentPurchasePrice,
            input.FilamentSpoolWeightInGrams,
            input.SalesMultiplier,
            input.ExtraFixedFeePerPrint,
            input.CostPerPrinterHour,
            currencyCode);

        var json = JsonSerializer.Serialize(dataToStore);

        await _browserJavascript.InvokeVoidAsync("localStorage.setItem", StorageKey, json);
    }
}

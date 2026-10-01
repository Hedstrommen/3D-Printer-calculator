using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using PrintCostCalculator.Core.Models;
using System.Text.Json;

namespace PrintCostCalculator.Web.Services;

/// <summary>
/// Stores the last inputs in the browser's local storage (private-encrypted by Blazor).
/// Swap this class in DI for a different storage (e.g. a database) if needed.
/// </summary>
public class BrowserUserInputStore : IUserInputStore
{
    private const string StorageKey = "printCostCalculatorLastInputs";

    private readonly ProtectedLocalStorage _browserLocalStorage;

    public BrowserUserInputStore(ProtectedLocalStorage browserLocalStorage)
    {
        _browserLocalStorage = browserLocalStorage;
    }

    public async Task<SavedPrintCostInput?> LoadAsync()
    {
        try
        {
            var storedJson = await _browserLocalStorage.GetAsync<string>(StorageKey);
            if (!storedJson.Success || storedJson.Value is null)
            {
                return null;
            }

            return JsonSerializer.Deserialize<SavedPrintCostInput>(storedJson.Value);
        }
        catch
        {
            // Storage might be unavailable (private mode, first render on server).
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
        await _browserLocalStorage.SetAsync(StorageKey, json);
    }
}

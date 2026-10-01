# 3D Print Price Calculator

A web app (usable on both desktop and phone) for calculating what a 3D print costs to make — and what you should sell it for.

Built in C# with ASP.NET Core (Blazor Server) and dependency injection throughout.

## Features

- **Filament cost** — enter spool price, spool weight, and model weight; get the material cost
- **Sales multiplier** — e.g. `3.0` means a 100 kr print sells for 300 kr
- **Optional fixed fee** — machine wear, nozzles, etc.
- **Optional printer-hour cost** — electricity and wear based on print duration
- **Currency switch** — SEK (default), EUR, USD
- **Remembers your inputs** — saved in your browser between visits
- **Phone-friendly** — installable as an app (PWA), responsive layout
- **Green & teal theme** with soft animations

## Run it

Requires the [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0).

```bash
cd src/PrintCostCalculator.Web
dotnet run
```

Then open the URL it prints (usually `http://localhost:5000`). Open the same URL on your phone (same Wi-Fi) to use it there, or host it anywhere ASP.NET Core runs.

## Project layout

```
src/
  PrintCostCalculator.Core/          # All calculation logic, no UI
    Models/
      PrintCostInput.cs              # What the user types in
      PrintCostResult.cs             # The price breakdown
    Services/
      IPrintCostCalculator.cs        # The calculation contract
      PrintCostCalculatorService.cs   # The default implementation
      PrintPriceFormatter.cs         # Currency formatting (SEK/EUR/USD)

  PrintCostCalculator.Web/           # The web UI (Blazor Server)
    Components/Pages/CalculatorPage.razor   # The whole UI
    Services/
      IUserInputStore.cs            # Contract for remembering inputs
      BrowserUserInputStore.cs      # Browser local-storage implementation
    wwwroot/css/app.css              # Green & teal theme
```

## How the price is calculated

```
pricePerGram = filamentPurchasePrice / filamentSpoolWeightInGrams
materialCost = pricePerGram * modelWeightInGrams

totalCost    = materialCost + extraFixedFeePerPrint + (costPerPrinterHour * printDurationInHours)
sellingPrice = totalCost * salesMultiplier
```

## Modding it

Everything is wired through dependency injection in `src/PrintCostCalculator.Web/Program.cs`:

```csharp
builder.Services.AddScoped<IPrintCostCalculator, PrintCostCalculatorService>();
```

Want a different pricing model? Write a new `IPrintCostCalculator` implementation and swap one line. Same goes for the input store (save to a database instead of the browser) and the currency formatter.

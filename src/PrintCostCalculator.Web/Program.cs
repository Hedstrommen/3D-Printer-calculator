using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PrintCostCalculator.Core.Services;
using PrintCostCalculator.Web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// ---- Dependency injection ----
// Swap any of these registrations to change behaviour without touching the UI.
builder.Services.AddScoped<IPrintCostCalculator, PrintCostCalculatorService>();
builder.Services.AddScoped<PrintPriceFormatter>();
builder.Services.AddScoped<IUserInputStore, BrowserUserInputStore>();

await builder.Build().RunAsync();

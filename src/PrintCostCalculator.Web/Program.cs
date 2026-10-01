using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PrintCostCalculator.Core.Services;
using PrintCostCalculator.Web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Mount the app (Main.razor, which contains the Router) into the
// <div id="app"> element in wwwroot/index.html.
builder.RootComponents.Add<Main>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// ---- Dependency injection ----
// Swap any of these registrations to change behaviour without touching the UI.
builder.Services.AddScoped<IPrintCostCalculator, PrintCostCalculatorService>();
builder.Services.AddScoped<PrintPriceFormatter>();
builder.Services.AddScoped<IUserInputStore, BrowserUserInputStore>();

await builder.Build().RunAsync();

using PrintCostCalculator.Core.Services;
using PrintCostCalculator.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// ---- Dependency injection ----
// Swap any of these registrations to change behaviour without touching the UI.
builder.Services.AddScoped<IPrintCostCalculator, PrintCostCalculatorService>();
builder.Services.AddScoped<PrintPriceFormatter>();
builder.Services.AddScoped<IUserInputStore, BrowserUserInputStore>();
builder.Services.AddProtectedBrowserStorage(); // encrypts saved inputs in the browser

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<PrintCostCalculator.Web.Components.App>()
   .AddInteractiveServerRenderMode();

app.Run();

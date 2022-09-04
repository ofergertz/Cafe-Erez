using Blazored.LocalStorage;
using Blazored.Modal;
using CafeErez.Client;
using CafeErez.Shared.Infrastructure.Localization;
using Infrastructure.Extensions.IOC;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using System.Globalization;
using static CafeErez.Shared.Constants.Constants;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddBlazoredModal();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();
builder.Services.RegisterClientComponents();

var host = builder.Build();

await InitializeLocalization(host);

await host.RunAsync();

async Task InitializeLocalization(WebAssemblyHost host)
{
    var storageService = host.Services.GetRequiredService<ILocalStorageService>();
    var culturePreference = await storageService.GetItemAsync<LanguageCode>(StorageConstants.LocalPreference);
    CultureInfo culture = new CultureInfo(culturePreference?.Code ?? "en-US");
    CultureInfo.DefaultThreadCurrentCulture = culture;
    CultureInfo.DefaultThreadCurrentUICulture = culture;
}
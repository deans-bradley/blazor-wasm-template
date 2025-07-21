using BlazorApp.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using BlazorApp.Utils.Services;
using Blazored.LocalStorage;
using MudBlazor.Services;
using BlazorApp.Client.ViewModels.Interfaces;
using BlazorApp.Client.ViewModels;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<AuthorizationMessageHandler>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// TODO: Use AutoFac for service registration
builder.Services.AddMudServices();
builder.Services.AddScoped<IHttpService, HttpService>();
builder.Services.AddTransient<IWeatherViewModel, WeatherViewModel>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore(); 
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

await builder.Build().RunAsync();
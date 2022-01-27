using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WordleBlazorWASMApp;
using WorldleGameEngine;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

var possibleWordles = builder.Configuration.GetSection("PossibleWordles").Get<List<string>>();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(possibleWordles);
builder.Services.AddSingleton<IGameEngine, GameEngine>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();

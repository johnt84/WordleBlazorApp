using WorldleGameEngine;

var builder = WebApplication.CreateBuilder(args);

var possibleWordles = builder.Configuration.GetSection("PossibleWordles").Get<List<string>>();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddSingleton(possibleWordles);
builder.Services.AddSingleton<IGameEngine, GameEngine>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

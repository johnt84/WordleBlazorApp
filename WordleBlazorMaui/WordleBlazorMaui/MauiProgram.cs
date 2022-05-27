using Microsoft.Extensions.Configuration;
using System.Reflection;
using WorldleGameEngine;

namespace WordleBlazorMaui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

        var a = Assembly.GetExecutingAssembly();
        using var stream = a.GetManifestResourceStream("WordleBlazorMaui.appsettings.json");

        var config = new ConfigurationBuilder()
          .AddJsonStream(stream)
          .Build();

        var possibleWordles = config.GetSection("PossibleWordles").Get<List<string>>();

        builder.Services.AddMauiBlazorWebView();
#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
#endif

        builder.Services.AddSingleton(possibleWordles);
        builder.Services.AddSingleton<IGameEngine, GameEngine>();

        return builder.Build();
	}
}

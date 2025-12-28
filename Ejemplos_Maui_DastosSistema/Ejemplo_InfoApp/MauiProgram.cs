using Ejemplo_InfoApp.Pages;
using Ejemplo_InfoApp.Services;
using Microsoft.Extensions.Logging;

namespace Ejemplo_InfoApp;

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
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        _=new FileLoggerService();

        builder.Services.AddSingleton<FileLoggerService>();
        builder.Services.AddSingleton<InfoSystemService>();

        builder.Services.AddSingleton<ThrowExceptionPage>();
        builder.Services.AddSingleton<MainPage>();

        

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}

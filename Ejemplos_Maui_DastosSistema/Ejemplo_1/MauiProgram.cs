using Ejemplo_1.Pages;
using Ejemplo_1.Services;
using Microsoft.Extensions.Logging;

namespace Ejemplo_1
{
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
            builder.Services.AddSingleton<StatusReport>();

            builder.Services.AddSingleton<ThrowExceptionPage>();
            builder.Services.AddSingleton<MainPage>();

            

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}

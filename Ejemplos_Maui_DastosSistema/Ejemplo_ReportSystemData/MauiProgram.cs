using Ejemplo_ReportSystemData.Services;
using Ejemplo_ReportSystemData.Utilities;
using Microsoft.Extensions.Logging;

namespace Ejemplo_ReportSystemData;

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

        string nombreArchivo = "mi_aplicacion.log";

        //Microsoft.Extensions.Http
        //builder.Services.AddHttpClient<LogClientReportApiService>().ConfigureHttpClient(client =>
        //    {
        //        client.Timeout = TimeSpan.FromSeconds(20);
        //    }            
        //);

        var cultureInfo = new System.Globalization.CultureInfo("es-ES");
        System.Globalization.CultureInfo.CurrentCulture = cultureInfo;
        System.Globalization.CultureInfo.CurrentUICulture = cultureInfo;

        builder.Services.AddHttpClient();

        /*
.AddTransientHttpErrorPolicy()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, attempt))         
         */

        string rutaLogs = Path.Combine(FileSystem.AppDataDirectory, nombreArchivo);

        builder.Logging.AddProvider(new FileLoggerProvider(rutaLogs));
        builder.Services.AddSingleton<ILogReaderService>(new LogReaderService(rutaLogs));
                
        builder.Services.AddSingleton<SOLoggerService>();

        builder.Services.AddSingleton<InfoSystemService>();

        builder.Services.AddSingleton<LogClientReportApiService>();

        return builder.Build();
    }
}

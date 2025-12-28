using Ejemplo_ReportSystemData.Pages;
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

        string rutaLogs = Path.Combine(FileSystem.AppDataDirectory, nombreArchivo);

        builder.Logging.AddProvider(new FileLoggerProvider(rutaLogs));
        builder.Services.AddSingleton<ILogReaderService>(new LogReaderService(rutaLogs));
                
        builder.Services.AddSingleton<SOLoggerService>();

        builder.Services.AddSingleton<InfoSystemService>();

        builder.Services.AddSingleton<LogClientReportApiService>();


        return builder.Build();
    }
}

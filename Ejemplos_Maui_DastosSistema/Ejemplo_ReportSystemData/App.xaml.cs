using Ejemplo_ReportSystemData.Pages;
using Ejemplo_ReportSystemData.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Reflection.Metadata;

namespace Ejemplo_ReportSystemData;

public partial class App : Application
{
    private readonly ILogger<MainPage> _logger;
    private readonly LogClientReportApiService _logReporter;
    private readonly InfoSystemService _infoSystemService;

    public App(ILogger<MainPage> logger, LogClientReportApiService logReporter)
    {
        InitializeComponent();

        _logger = logger;
        _logReporter = logReporter;

        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }

    protected override void OnStart()
    {
        base.OnStart(); // Siempre llama a la base primero

        // Ejecutamos la lógica de logs sin bloquear el hilo de UI
        // y sin usar .GetResult() que causa Deadlocks
        //try
        //{
        //    Task.Run(async () => await InicializarLogsAsync());
        //}
        //catch (Exception ex)
        //{ 
        //}

        //MainThread.BeginInvokeOnMainThread(async () =>
        //{
        //    // Esperar a que la pantalla se renderice completamente
        //    await Task.Delay(2000); // 2 segundos

        //    // AHORA ejecutar las operaciones pesadas en background
        //    await Task.Run(async () => await InicializarLogsAsync());
        //});

        Dispatcher.Dispatch(async () =>
        {
            // Esperar a que la UI esté lista
            await Task.Delay(2000);

            // Ejecutar en background
            await Task.Run(async () => await InicializarLogsAsync());
        });
    }

    private async Task InicializarLogsAsync()
    {

        try
        {
            _logger.LogInformation("OnStart(): Enviando reporte");
            
            
            await _logReporter.SendAndClearFileLogReportAsync();
            await _logReporter.SendCatlogReportAsync();
            
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[App] Error enviando reporte de logs: {ex.Message}");
        }
    }

    private async void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        try
        {
            var exception = e.ExceptionObject as Exception;

            if (exception != null)
            {
                _logger.LogError(exception, "Excepción no manejada capturada");

                _logger.LogError($"Mensaje: {exception.Message}");
                _logger.LogError($"Tipo: {exception.GetType().Name}");
                _logger.LogError($"StackTrace: {exception.StackTrace}");
                _logger.LogError($"Source: {exception.Source}");

                if (exception.InnerException != null)
                {
                    _logger.LogError($"Inner Exception: {exception.InnerException.Message}");
                    _logger.LogError($"Inner StackTrace: {exception.InnerException.StackTrace}");
                }

                _logger.LogError($"¿Terminará la aplicación?: {e.IsTerminating}");

            }
            else
            {
                _logger.LogError($"Excepción no estándar: {e.ExceptionObject}");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error al loguear excepción no manejada: {ex.Message}");
        }
    }

    private async void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
    {
        if (e != null)
        {
            _logger.LogError($"Captura excepciones de Tasks que nadie await-eó.UNHANDLED");
            _logger.LogError(e.Exception.Message);
            _logger.LogError(e.Exception.StackTrace);
            _logger.LogError($"URL actual: {ContextService.URLActual}");
            _logger.LogError($"Memory status actual: {_infoSystemService.GetMemoryInfoText()}");
        }
    }

}
using Ejemplo_ReportSystemData.Pages;
using Ejemplo_ReportSystemData.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Reflection.Metadata;

namespace Ejemplo_ReportSystemData
{
    public partial class App : Application
    {
        private readonly ILogger<MainPage> _logger;
        private readonly LogClientReportApiService _logReporter;

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
            base.OnStart();

            try
            {
                _logger.LogInformation("OnStart(): Enviando reporte");
                //_logReporter.SendAndClearFileLogReportAsync().GetAwaiter().GetResult();
                //
                Task.Run(async () =>
                {
                    try
                    {
                        await _logReporter.SendAndClearFileLogReportAsync();
                        await _logReporter.SendCatlogReportAsync();
                    }
                    catch (Exception ex)
                    {
                        // Importante: Loguear aquí porque dentro de Task.Run 
                        // los errores no saltan al try-catch de afuera.
                        System.Diagnostics.Debug.WriteLine($"[App] Error en tarea de reporte: {ex.Message}");
                    }
                });
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
                // Si falla el logging, al menos intentar escribir en Debug
                System.Diagnostics.Debug.WriteLine($"Error al loguear excepción no manejada: {ex.Message}");
            }
        }

        private async void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            ////Captura excepciones de Tasks que nadie await-eó.

            //Debug.WriteLine("🔥 ANDROID UNHANDLED");
            //Debug.WriteLine(e.Exception.Message);
            //Debug.WriteLine(e.Exception.StackTrace);

            //_soLogger.Log(e.Exception);
        }

    }
}
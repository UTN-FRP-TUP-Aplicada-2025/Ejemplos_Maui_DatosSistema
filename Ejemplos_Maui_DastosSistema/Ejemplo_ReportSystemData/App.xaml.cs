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

            System.Diagnostics.Debug.WriteLine("OnStart");

            try
            {
                //string Logcat = _logReader.ReadLogs();

                //_logReporter.SendLogReportAsync("","").GetAwaiter().GetResult();

                //_logReader.ClearLogs();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[App] Error enviando reporte de logs: {ex.Message}");
            }
            
        }

        private async void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //Captura excepciones no manejadas del dominio de la app.



           
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
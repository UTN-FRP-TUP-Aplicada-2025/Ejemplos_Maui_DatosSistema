using Ejemplo_ReportSystemData.Services;
using Microsoft.Extensions.Logging;

namespace Ejemplo_ReportSystemData.Pages;

public partial class MainPage : ContentPage
{

    private readonly LogClientReportApiService _logReporter;
    private readonly ILogger<MainPage> _logger;

    public MainPage(ILogger<MainPage> logger, SOLoggerService sologger, LogClientReportApiService logReporter)
    {
        InitializeComponent();
        BindingContext = this;
        
        _logReporter = logReporter;
        _logger = logger;
    }

    private void OnFileLogClicked(object? sender, EventArgs e)
    {
        _logger.LogInformation("Prueba");
    }

    private void OnFileLogTestAPIClicked(object? sender, EventArgs e)
    {

        System.Diagnostics.Debug.WriteLine("OnStart");

        try
        {
            _logReporter.SendCatlogReportAsync().GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[App] Error enviando reporte de logs: {ex.Message}");
        }
    }

    private void OnLogcastTestAPIClicked(object? sender, EventArgs e)
    {

        System.Diagnostics.Debug.WriteLine("OnStart");

        try
        {
            _logReporter.SendCatlogReportAsync().GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[App] Error enviando reporte de logs: {ex.Message}");
        }
    }

    private void OnThrowExceptionPageClicked(object? sender, EventArgs e)
    {

        Shell.Current.GoToAsync(nameof(ThrowExceptionPage));
    }
}

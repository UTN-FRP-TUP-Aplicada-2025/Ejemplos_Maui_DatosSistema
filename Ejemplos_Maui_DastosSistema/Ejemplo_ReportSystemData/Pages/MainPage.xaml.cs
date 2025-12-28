using Ejemplo_ReportSystemData.Services;
using Microsoft.Extensions.Logging;

namespace Ejemplo_ReportSystemData.Pages;

public partial class MainPage : ContentPage
{
    private readonly LogClientReportApiService _logReporter;
    private readonly ILogger<MainPage> _logger;

    public MainPage(ILogger<MainPage> logger, LogClientReportApiService logReporter)
    {
        InitializeComponent();
        this.BindingContext = this;
        
        _logReporter = logReporter;
        _logger = logger;
    }

    private void OnFileLogClicked(object? sender, EventArgs e)
    {
        _logger.LogInformation("Prueba");
    }

    private void OnFileLogTestAPIClicked(object? sender, EventArgs e)
    {
        System.Diagnostics.Debug.WriteLine("OnFileLogTestAPIClicked");

        try
        {
            _logReporter.SendAndClearFileLogReportAsync().GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[App] Error enviando reporte de logs: {ex.Message}");
        }
    }

    private void OnLogcastTestAPIClicked(object? sender, EventArgs e)
    {
        System.Diagnostics.Debug.WriteLine("OnLogcastTestAPIClicked");

        try
        {
            _logReporter.SendCatlogReportAsync().GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[App] Error enviando reporte de logs: {ex.Message}");
        }
    }

    async private void OnThrowExceptionPageClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(Pages.ThrowExceptionPage));
    }
}

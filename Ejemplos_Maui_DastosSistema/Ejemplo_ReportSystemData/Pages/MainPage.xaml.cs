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
            Task.Run( async () => await _logReporter.SendAndClearFileLogReportAsync() );            
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
            Task.Run(async () => await _logReporter.SendCatlogReportAsync());
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[App] Error enviando reporte de logs: {ex.Message}");
        }
    }

    async private void OnThrowExceptionClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(Pages.ThrowExceptionPage));
    }

    async private void OnWebViewClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(Pages.WebViewPage));
    }
}

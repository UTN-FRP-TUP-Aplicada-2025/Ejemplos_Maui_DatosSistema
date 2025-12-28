using Ejemplo_ReportSystemData.DTOs;
using System.Text.Json;

namespace Ejemplo_ReportSystemData.Services;

public class LogClientReportApiService
{
    private readonly InfoSystemService _systemInfo=default!;

    public LogClientReportApiService(InfoSystemService systemInfo)
    {
        _systemInfo = systemInfo;
    }

    async public Task SendCatlogReportAsync()
    {
        string apiUrl = "https://hxbt1xfz-7236.brs.devtunnels.ms/api/ReporLogs";

        var reporte = new ReportDTO
        {
            Network = _systemInfo.GetNetworkType(),
            IdDevice = _systemInfo.GetDeviceId(),
            InfoMemory = _systemInfo.GetMemoryInfoText(),
            InfoDevice = _systemInfo.GetDeviceInfoText(),
            InfoProcessor = _systemInfo.GetProcessorInfoText(),
            InfoApp = "1.0.0",
            Containt = _systemInfo.GetContentFileLog(),
            TipoContaint = "filelog",             
        };

        var jsonContent = JsonSerializer.Serialize(reporte);
        var request = new HttpRequestMessage(HttpMethod.Post, apiUrl)
        {
           Content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json")
        };

        try
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Reporte enviado exitosamente.");
            }
            else
            {
                Console.WriteLine($"Error al enviar el reporte. Código de estado: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            // Manejar la excepción según sea necesario
            Console.WriteLine($"Error al enviar el reporte: {ex.Message}");
        }

        //response.EnsureSuccessStatusCode();
        //var responseContent = await response.Content.ReadAsStringAsync();


    }

    async public Task SendAndClearFileLogReportAsync()
    {
        string apiUrl = "https://hxbt1xfz-7236.brs.devtunnels.ms/api/ReporLogs";

        var reporte = new ReportDTO
        {
            Network = _systemInfo.GetNetworkType(),
            IdDevice = _systemInfo.GetDeviceId(),
            InfoMemory = _systemInfo.GetMemoryInfoText(),
            InfoDevice = _systemInfo.GetDeviceInfoText(),
            InfoProcessor = _systemInfo.GetProcessorInfoText(),
            InfoApp = "1.0.0",
            Containt = _systemInfo.GetContentLogcat(),
            TipoContaint = "logcat",
        };

        var jsonContent = JsonSerializer.Serialize(reporte);
        var request = new HttpRequestMessage(HttpMethod.Post, apiUrl)
        {
            Content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json")
        };

        try
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Reporte enviado exitosamente.");
            }
            else
            {
                Console.WriteLine($"Error al enviar el reporte. Código de estado: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            // Manejar la excepción según sea necesario
            Console.WriteLine($"Error al enviar el reporte: {ex.Message}");
        }

        //response.EnsureSuccessStatusCode();
        //var responseContent = await response.Content.ReadAsStringAsync();


    }
}

using Ejemplo_ReportSystemData.DTOs;
using System.Net.Http;
using System.Text.Json;

namespace Ejemplo_ReportSystemData.Services;

public class LogClientReportApiService
{
    private readonly InfoSystemService _systemInfo=default!;
    private readonly HttpClient _httpClient;
    
    // ⭐ CONSTANTE PARA TIMEOUT
    private const int TIMEOUT_SECONDS = 20;

    public LogClientReportApiService(InfoSystemService systemInfo, HttpClient httpClient)
    {
        _systemInfo = systemInfo;
        _httpClient = httpClient;
    }

    async public Task SendCatlogReportAsync()
    {
        //string apiUrl = "https://hxbt1xfz-7236.brs.devtunnels.ms/api/ReporLogs";
        string apiUrl = "https://GeometriaFernando.somee.com/api/ReporLogs";

        var reporte = new ReportDTO
        {
            Fecha = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm"),
            VersionApp = AppInfo.VersionString,
            CodeVersionApp = AppInfo.BuildString,
            Network = _systemInfo.GetNetworkType(),
            IdDevice = _systemInfo.GetDeviceId(),
            OSDevice = DeviceInfo.VersionString,
            PlatformDevice = DeviceInfo.Platform.ToString(),
            ManufacturerDevice = DeviceInfo.Manufacturer,
            IdiomDevice = DeviceInfo.Idiom.ToString(),
            InfoMemory = _systemInfo.GetMemoryInfoText(),
            InfoDevice = _systemInfo.GetDeviceInfoText(),
            InfoProcessor = _systemInfo.GetProcessorInfoText(),
            Containt = await _systemInfo.GetContentLogcatAsync(),
            TipoContain = "LOGCAT",
            URL = apiUrl
        };

        var jsonContent = JsonSerializer.Serialize(reporte);
        int jsonSize = System.Text.Encoding.UTF8.GetByteCount(jsonContent);

        var request = new HttpRequestMessage(HttpMethod.Post, apiUrl)
        {
           Content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json")
        };

        try
        {
            //using var httpClient = new HttpClient();
            //httpClient.Timeout = TimeSpan.FromSeconds(30);
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(TIMEOUT_SECONDS));
            HttpResponseMessage response = await _httpClient.SendAsync(request, cts.Token);

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
        //string apiUrl = "https://hxbt1xfz-7236.brs.devtunnels.ms/api/ReporLogs";
        string apiUrl = "https://GeometriaFernando.somee.com/api/ReporLogs";

        var reporte = new ReportDTO
        {
            Fecha = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm"),
            VersionApp = AppInfo.VersionString,
            CodeVersionApp = AppInfo.BuildString,
            Network = _systemInfo.GetNetworkType(),
            IdDevice = _systemInfo.GetDeviceId(),
            OSDevice = DeviceInfo.VersionString,
            PlatformDevice = DeviceInfo.Platform.ToString(),
            ManufacturerDevice = DeviceInfo.Manufacturer,
            IdiomDevice = DeviceInfo.Idiom.ToString(),
            InfoMemory = _systemInfo.GetMemoryInfoText(),
            InfoDevice = _systemInfo.GetDeviceInfoText(),
            InfoProcessor = _systemInfo.GetProcessorInfoText(),
            Containt = await _systemInfo.GetContentFileLogAsync(),
            TipoContain = "FILELOG",
            URL = apiUrl
        };

        _systemInfo.ClearFileLog();

        var jsonContent = JsonSerializer.Serialize(reporte);
        int jsonSize = System.Text.Encoding.UTF8.GetByteCount(jsonContent);


        var request = new HttpRequestMessage(HttpMethod.Post, apiUrl)
        {
            Content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json")
        };

        try
        {
            // using var httpClient = new HttpClient() { Timeout = TimeSpan.FromSeconds(30) }; 

#if DEBUG
            //var handler = new HttpClientHandler();
            //handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            //using var httpClient = new HttpClient(handler);
#else
    //using var httpClient = new HttpClient();
#endif
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(TIMEOUT_SECONDS));
            HttpResponseMessage response = await _httpClient.SendAsync(request, cts.Token);

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

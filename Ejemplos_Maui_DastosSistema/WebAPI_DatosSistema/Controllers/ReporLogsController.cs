using Microsoft.AspNetCore.Mvc;
using WebAPI_DatosSistema.DTOs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI_DatosSistema.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReporLogsController : ControllerBase
{
    static List<ReportDTO> reportDTOs = new List<ReportDTO>();

    [HttpGet]
    async public Task<ActionResult<List<ReportDTO>>> Get()
    {
        return Ok(reportDTOs);
    }

    [HttpPost]
    /// <summary>
    /// Agrega un nuevo reporte a la lista de reportes.
    /// </summary>
    /// <param name="reportDTO">El objeto ReportDTO que contiene la información del reporte.</param>
    /// <returns>Un resultado de acción que indica si la operación fue exitosa.</returns>
    async public Task<ActionResult> Post([FromBody] ReportDTO reportDTO)
    {
        reportDTOs.Add(reportDTO);
        //{"network":"WiFi","idDevice":"ebd7cba2072c42d5","infoMemory":"TotalMemoryMB: 7\nFreeMemoryMB: 3\nUsedMemoryMB: 4\nMaxMemoryMB: 256\nSystemLowMemory: False\nSystemAvailableMB: 1575\nSystemTotalMB: 3672\n","infoDevice":"Model: moto g42\nManufacturer: motorola\nOSVersion: 13\nDeviceName: Physical\nDevice: Android\n","infoProcessor":"AvailableProcessors: 8\nProcessorCount: 8\nUsageProcessor: System.Environment\u002BProcessCpuUsage\n","infoApp":"1.0.0","containt":"","tipoContaint":""}
        return Ok();
    }
}

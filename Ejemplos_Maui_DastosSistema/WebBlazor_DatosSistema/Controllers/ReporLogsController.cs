using Microsoft.AspNetCore.Mvc;
using WebBlazor_DatosSistema.DTOs;

namespace WebBlazor_DatosSistema.Controllers;

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
    async public Task<ActionResult> Post([FromBody] ReportDTO reportDTO)
    {
      reportDTOs.Add(reportDTO);
       return Ok();
    }
}

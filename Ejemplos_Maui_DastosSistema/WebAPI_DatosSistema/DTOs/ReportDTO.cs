using System.Text.Json.Serialization;

namespace WebAPI_DatosSistema.DTOs;

public class ReportDTO
{
    
    [JsonPropertyName("network")]
    public string Network { get; set; }

    [JsonPropertyName("idDevice")]
    public string IdDevice { get; set; }

    [JsonPropertyName("infoMemory")]
    public string InfoMemory { get; set; }

    [JsonPropertyName("infoDevice")]
    public string InfoDevice { get; set; }

    [JsonPropertyName("infoProcessor")]
    public string InfoProcessor { get; set; }

    [JsonPropertyName("infoApp")]
    public string InfoApp { get; set; }

    [JsonPropertyName("containt")]
    public string Containt { get; set; }

    [JsonPropertyName("tipoContaint")]
    public string TipoContaint { get; set; }

}

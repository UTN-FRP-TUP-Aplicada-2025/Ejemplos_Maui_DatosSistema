using System.Text.Json.Serialization;

namespace WebAPI_DatosSistema.DTOs;

public class ReportDTO
{
    [JsonPropertyName("id")]
    public int? Id { get; set; }

    [JsonPropertyName("fecha")]
    public string Fecha { get; set; }

    [JsonPropertyName("versionApp")]
    public string VersionApp { get; set; }

    [JsonPropertyName("codeVersionApp")]
    public string CodeVersionApp { get; set; }

    [JsonPropertyName("network")]
    public string Network { get; set; }

    [JsonPropertyName("idDevice")]
    public string IdDevice { get; set; }

    [JsonPropertyName("osDevice")]
    public string OSDevice { get; set; }

    [JsonPropertyName("platformDevice")]
    public string PlatformDevice { get; set; }

    [JsonPropertyName("manufacturerDevice")]
    public string ManufacturerDevice { get; set; }

    [JsonPropertyName("idiomDevice")]
    public string IdiomDevice { get; set; }

    [JsonPropertyName("infoMemory")]
    public string InfoMemory { get; set; }

    [JsonPropertyName("infoDevice")]
    public string InfoDevice { get; set; }

    [JsonPropertyName("infoProcessor")]
    public string InfoProcessor { get; set; }

    [JsonPropertyName("containt")]
    public string Contain { get; set; }

    [JsonPropertyName("tipoContaint")]
    public string TipoContain { get; set; }

    [JsonPropertyName("url")]
    public string URL { get; set; }

}

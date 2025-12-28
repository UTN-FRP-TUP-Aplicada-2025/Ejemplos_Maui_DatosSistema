namespace Ejemplo_ReportSystemData.Services;

public class LogReaderService : ILogReaderService
{
    private readonly string _filePath;

    public LogReaderService(string filePath) => _filePath = filePath;

    public string ReadLogs() => File.Exists(_filePath) ? File.ReadAllText(_filePath) : "Archivo vacío";

    public void ClearLogs() { if (File.Exists(_filePath)) File.Delete(_filePath); }
}

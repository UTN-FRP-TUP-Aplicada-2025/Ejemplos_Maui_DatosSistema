namespace Ejemplo_ReportSystemData.Services;

public interface ILogReaderService
{
    string ReadLogs();
    Task<string> ReadLogsAsync(int maxLines = 100);
    void ClearLogs();
}
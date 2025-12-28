namespace Ejemplo_ReportSystemData.Services;

public interface ILogReaderService
{
    string ReadLogs();
    string ReadLogs(int maxLines = 50);
    void ClearLogs();
}
namespace Ejemplo_ReportSystemData.Services;

public interface ILogReaderService
{
    string ReadLogs();
    void ClearLogs();
}
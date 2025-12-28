namespace Ejemplo_ReportSystemData.Services;

public class LogReaderService : ILogReaderService
{
    private const int DEFAULT_MAX_LINES = 200;
    private const int MAX_LINE_LENGTH = 1000;

    private readonly string _filePath;

    public LogReaderService(string filePath) => _filePath = filePath;

    public string ReadLogs() => File.Exists(_filePath) ? File.ReadAllText(_filePath) : "Archivo vacío";

    public string ReadLogs(int maxLines = DEFAULT_MAX_LINES)
    {
        if (!File.Exists(_filePath))
            return string.Empty;

        try
        {
            var allLines = File.ReadAllLines(_filePath);

            // Tomar solo las últimas N líneas
            var recentLines = allLines
                .TakeLast(maxLines)
                .Select(line => TruncateLine(line, MAX_LINE_LENGTH));

            return string.Join(Environment.NewLine, recentLines);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[LogReader] Error leyendo archivo: {ex.Message}");
            return $"[Error reading log: {ex.Message}]";
        }
    }

    public string ReadCriticalLogs(int maxLines = 50)
    {
        if (!File.Exists(_filePath))
            return string.Empty;

        try
        {
            var allLines = File.ReadAllLines(_filePath);

            var criticalLines = allLines
                .Where(line => line.Contains("[Error]") ||
                              line.Contains("[Critical]") ||
                              line.Contains("[Exception]") ||
                              line.Contains("Exception:"))
                .TakeLast(maxLines)
                .Select(line => TruncateLine(line, MAX_LINE_LENGTH));

            return string.Join(Environment.NewLine, criticalLines);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[LogReader] Error leyendo logs críticos: {ex.Message}");
            return $"[Error reading critical logs: {ex.Message}]";
        }
    }

    private string TruncateLine(string line, int maxLength)
    {
        if (line.Length <= maxLength)
            return line;

        return line.Substring(0, maxLength) + "... [truncated]";
    }

    public void ClearLogs()
    {
        try
        {
            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
                System.Diagnostics.Debug.WriteLine($"[LogReader] ✅ Archivo de logs eliminado: {_filePath}");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[LogReader] ❌ Error eliminando logs: {ex.Message}");
        }
    }
}

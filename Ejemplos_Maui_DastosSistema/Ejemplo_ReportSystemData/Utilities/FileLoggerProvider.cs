using Microsoft.Extensions.Logging;


namespace Ejemplo_ReportSystemData.Utilities;

public class FileLoggerProvider : ILoggerProvider
{
    private readonly string _filePath;

    public FileLoggerProvider(string filePath) => _filePath = filePath;

    public ILogger CreateLogger(string categoryName) => new SimpleFileLogger(_filePath);

    public void Dispose() { }
}

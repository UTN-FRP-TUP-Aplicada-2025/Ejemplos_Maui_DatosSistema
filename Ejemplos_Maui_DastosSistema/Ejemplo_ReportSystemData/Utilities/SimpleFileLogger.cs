using Microsoft.Extensions.Logging;

namespace Ejemplo_ReportSystemData.Utilities;

public class SimpleFileLogger : ILogger
{
    private readonly string _filePath;
    private static readonly object _lock = new();

    public SimpleFileLogger(string filePath) => _filePath = filePath;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel)) return;

        var mensaje = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{logLevel}] {formatter(state, exception)}";
        if (exception != null) mensaje += Environment.NewLine + exception.ToString();

        lock (_lock)
        {
            File.AppendAllText(_filePath, mensaje + Environment.NewLine);
        }
    }
}

#if ANDROID
using Android.App;
using Android.Content;
using Bumptech.Glide.Load.Model;
#endif
using System.Diagnostics;
using System.Text;

namespace Ejemplo_InfoApp.Services;

public class FileLoggerService
{
    private readonly FileLogger _logger = new();
    private static FileLoggerService? _instance;

    public FileLoggerService()
    {
        _instance = this;
        // Log de inicialización para verificar que el servicio se creó
        _logger.Log("FileLoggerService inicializado", LogLevel.Info);
    }

    /// <summary>
    /// Obtiene la instancia singleton del servicio
    /// </summary>
    public static FileLoggerService? Instance => _instance;

    /// <summary>
    /// Registra un mensaje en el log
    /// </summary>
    public void LogMessage(string message, LogLevel level = LogLevel.Debug)
    {
        _logger.Log(message, ConvertLogLevel((Microsoft.Extensions.Logging.LogLevel)level));
    }

    /// <summary>
    /// Registra una excepción
    /// </summary>
    public void LogException(Exception ex, string message = "")
    {
        var errorMessage = string.IsNullOrEmpty(message)
            ? $"{ex.GetType().Name}: {ex.Message}\n{ex.StackTrace}"
            : $"{message}\n{ex.GetType().Name}: {ex.Message}\n{ex.StackTrace}";

        _logger.Log(errorMessage, LogLevel.Error);
    }

    /// <summary>
    /// Obtiene el contenido del log actual
    /// </summary>
    public string GetLogContent()
    {
        return _logger.ReadLog();
    }

    /// <summary>
    /// Obtiene las últimas N líneas del log
    /// </summary>
    public string GetLogLastLines(int lines = 100)
    {
        return _logger.ReadLastLines(lines);
    }

    /// <summary>
    /// Obtiene la ruta del archivo de log
    /// </summary>
    public string GetLogFilePath()
    {
        return _logger.GetLogFilePath();
    }

    /// <summary>
    /// Limpia los logs
    /// </summary>
    public void ClearLogs()
    {
        _logger.ClearLog();
    }

    /// <summary>
    /// Obtiene información de diagnóstico del archivo de log
    /// </summary>
    public string GetLogDiagnostics()
    {
        try
        {
            var logPath = GetLogFilePath();
            var diagnostics = new StringBuilder();

            diagnostics.AppendLine("=== DIAGNÓSTICO DE LOGS ===");
            diagnostics.AppendLine($"Ruta del archivo: {logPath}");
            diagnostics.AppendLine($"Archivo existe: {File.Exists(logPath)}");

            if (File.Exists(logPath))
            {
                var fileInfo = new FileInfo(logPath);
                diagnostics.AppendLine($"Tamaño del archivo: {fileInfo.Length} bytes");
                diagnostics.AppendLine($"Última modificación: {fileInfo.LastWriteTime}");
                diagnostics.AppendLine($"Instancia del servicio: {(Instance != null ? "Inicializada" : "NULL")}");
            }
            else
            {
                diagnostics.AppendLine("⚠️ El archivo de log no existe aún");
                diagnostics.AppendLine($"Instancia del servicio: {(Instance != null ? "Inicializada" : "NULL")}");
            }

            return diagnostics.ToString();
        }
        catch (Exception ex)
        {
            return $"Error obteniendo diagnósticos: {ex.Message}";
        }
    }

#if ANDROID
    /// <summary>
    /// Lee los logs de Android Logcat (últimas líneas)
    /// </summary>
    public string GetAndroidLogcat(int lines = 500)
    {
        try
        {
            var process = new ProcessStartInfo
            {
                FileName = "logcat",
                Arguments = $"-t {lines}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            using (var proc = Process.Start(process))
            {
                if (proc != null)
                {
                    var output = proc.StandardOutput.ReadToEnd();
                    proc.WaitForExit();
                    return output;
                }
            }
        }
        catch (Exception ex)
        {
            LogException(ex, "Error al leer Logcat de Android");
        }

        return "No se pudo leer Logcat";
    }

    /// <summary>
    /// Filtra los logs de Logcat por nombre de aplicación
    /// </summary>
    public string GetAndroidLogcatFiltered(string packageName, int lines = 500)
    {
        try
        {
            var process = new ProcessStartInfo
            {
                FileName = "logcat",
                Arguments = $"-t {lines} | grep {packageName}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            using (var proc = Process.Start(process))
            {
                if (proc != null)
                {
                    var output = proc.StandardOutput.ReadToEnd();
                    proc.WaitForExit();
                    return output;
                }
            }
        }
        catch (Exception ex)
        {
            LogException(ex, "Error al filtrar Logcat de Android");
        }

        return "No se pudo leer Logcat filtrado";
    }

    /// <summary>
    /// Exporta los logs de Logcat a un archivo
    /// </summary>
    public bool ExportAndroidLogcat(string filePath, int lines = 500)
    {
        try
        {
            var logcat = GetAndroidLogcat(lines);
            File.WriteAllText(filePath, logcat);
            LogMessage($"Logcat exportado a: {filePath}");
            return true;
        }
        catch (Exception ex)
        {
            LogException(ex, "Error al exportar Logcat");
            return false;
        }
    }
#endif

    /// <summary>
    /// Convierte LogLevel a nuestro enum
    /// </summary>
    private LogLevel ConvertLogLevel(Microsoft.Extensions.Logging.LogLevel level)
    {
        return level switch
        {
            Microsoft.Extensions.Logging.LogLevel.Debug => LogLevel.Debug,
            Microsoft.Extensions.Logging.LogLevel.Information => LogLevel.Info,
            Microsoft.Extensions.Logging.LogLevel.Warning => LogLevel.Warning,
            Microsoft.Extensions.Logging.LogLevel.Error => LogLevel.Error,
            Microsoft.Extensions.Logging.LogLevel.Critical => LogLevel.Critical,
            _ => LogLevel.Info
        };
    }
}

/// <summary>
/// Adaptador para redirigir Debug.WriteLine a FileLogger
/// </summary>
internal class FileLogTraceListener : TraceListener
{
    private readonly FileLogger _logger;

    public FileLogTraceListener(FileLogger logger)
    {
        _logger = logger;
    }

    public override void Write(string? message)
    {
        if (!string.IsNullOrEmpty(message))
        {
            _logger.Log(message.Trim(), LogLevel.Debug);
        }
    }

    public override void WriteLine(string? message)
    {
        if (!string.IsNullOrEmpty(message))
        {
            _logger.Log(message.Trim(), LogLevel.Debug);
        }
    }
}
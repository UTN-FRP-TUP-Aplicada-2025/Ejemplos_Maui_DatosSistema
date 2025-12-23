using System;
using System.Collections.Generic;
using System.Text;

namespace Ejemplo_1.Services;

public enum LogLevel
{
    Debug,
    Info,
    Warning,
    Error,
    Critical
}

public class FileLogger
{
    private readonly string _logFilePath;
    private readonly object _lockObject = new();
    private const string LogFileName = "app_log.txt";
    private const int MaxLogSizeBytes = 5 * 1024 * 1024; // 5 MB

    public FileLogger()
    {
        _logFilePath = Path.Combine(FileSystem.AppDataDirectory, LogFileName);
    }

    /// <summary>
    /// Registra un mensaje en el archivo de log con flush inmediato
    /// </summary>
    public void Log(string message, LogLevel level = LogLevel.Info)
    {
        try
        {
            lock (_lockObject)
            {
                // Verificar si el archivo es muy grande y rotarlo
                if (File.Exists(_logFilePath))
                {
                    var fileInfo = new FileInfo(_logFilePath);
                    if (fileInfo.Length > MaxLogSizeBytes)
                    {
                        RotateLog();
                    }
                }

                var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                var logEntry = $"[{timestamp}] [{level}] {message}{Environment.NewLine}";

                // Usar FileStream para garantizar escritura inmediata en disco
                using (var fs = new FileStream(_logFilePath, FileMode.Append, FileAccess.Write, FileShare.Read))
                using (var writer = new StreamWriter(fs, Encoding.UTF8))
                {
                    writer.Write(logEntry);
                    writer.Flush();
                    fs.Flush();
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error escribiendo log: {ex.Message}");
        }
    }

    /// <summary>
    /// Lee el contenido completo del log
    /// </summary>
    public string ReadLog()
    {
        try
        {
            lock (_lockObject)
            {
                if (File.Exists(_logFilePath))
                {
                    return File.ReadAllText(_logFilePath, Encoding.UTF8);
                }
            }
        }
        catch (Exception ex)
        {
            return $"Error leyendo log: {ex.Message}";
        }

        return "Log vacío";
    }

    /// <summary>
    /// Lee las últimas N líneas del log
    /// </summary>
    public string ReadLastLines(int lines = 100)
    {
        try
        {
            lock (_lockObject)
            {
                if (!File.Exists(_logFilePath))
                    return "Log vacío";

                var allLines = File.ReadAllLines(_logFilePath, Encoding.UTF8);
                var startIndex = Math.Max(0, allLines.Length - lines);
                var lastLines = allLines[startIndex..];

                return string.Join(Environment.NewLine, lastLines);
            }
        }
        catch (Exception ex)
        {
            return $"Error leyendo log: {ex.Message}";
        }
    }

    /// <summary>
    /// Obtiene la ruta del archivo de log
    /// </summary>
    public string GetLogFilePath()
    {
        return _logFilePath;
    }

    /// <summary>
    /// Limpia el contenido del log
    /// </summary>
    public void ClearLog()
    {
        try
        {
            lock (_lockObject)
            {
                if (File.Exists(_logFilePath))
                {
                    File.Delete(_logFilePath);
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error limpiando log: {ex.Message}");
        }
    }

    /// <summary>
    /// Rota el archivo de log cuando alcanza el tamaño máximo
    /// </summary>
    private void RotateLog()
    {
        try
        {
            var directory = Path.GetDirectoryName(_logFilePath) ?? FileSystem.AppDataDirectory;
            var fileName = Path.GetFileNameWithoutExtension(_logFilePath);
            var extension = Path.GetExtension(_logFilePath);
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            var backupPath = Path.Combine(directory, $"{fileName}_{timestamp}{extension}");

            File.Move(_logFilePath, backupPath);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error rotando log: {ex.Message}");
        }
    }
}
#if ANDROID
using Android.App;
using Android.Content;


#endif
using System;
using System.Collections.Generic;
using System.Text;
using Ejemplo_ReportSystemData.Utilities;

namespace Ejemplo_ReportSystemData.Services;

public class InfoSystemService
{

    private readonly ILogReaderService _logReaderService;
    private readonly SOLoggerService _soLoggerService;

    public InfoSystemService(ILogReaderService logReaderService, SOLoggerService soLoggerService)
    {
        _logReaderService = logReaderService;
        _soLoggerService = soLoggerService;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public string GetNetworkType()
    {
        string networkType = Connectivity.Current.NetworkAccess switch
        {
            NetworkAccess.Internet =>
                Connectivity.Current.ConnectionProfiles.Contains(ConnectionProfile.WiFi)
                    ? "WiFi"
                    : Connectivity.Current.ConnectionProfiles.Contains(ConnectionProfile.Cellular)
                        ? "Cellular"
                        : "Other",
            _ => "NoInternet"
        };
        return networkType;
    }

    public string GetDeviceId()
    {
        try
        {
#if ANDROID
            if (OperatingSystem.IsAndroidVersionAtLeast(28))
            {
                var androidId = Android.Provider.Settings.Secure.GetString(
                    Android.App.Application.Context.ContentResolver,
                    Android.Provider.Settings.Secure.AndroidId
                );
                return androidId ?? "Unknown";
            }
            else
            {
                var androidId = Android.Provider.Settings.Secure.GetString(Android.App.Application.Context.ContentResolver, Android.Provider.Settings.Secure.AndroidId
                    );
                return androidId ?? "Unknown";
            }
#else
            return "N/A";
#endif
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[DiagnosticsCollector] Error obteniendo Device ID: {ex.Message}");
            return "Error";
        }
    }

    public Dictionary<string, string> GetMemoryInfo()
    {
        var dic = new Dictionary<string, string>();
        try
        {
#if ANDROID
            var runtime = Java.Lang.Runtime.GetRuntime();
            var totalMemory = runtime.TotalMemory();
            var freeMemory = runtime.FreeMemory();
            var maxMemory = runtime.MaxMemory();
            var usedMemory = totalMemory - freeMemory;

            var memInfo = new ActivityManager.MemoryInfo();
            var activityManager = Android.App.Application.Context.GetSystemService(Context.ActivityService) as ActivityManager;
            activityManager?.GetMemoryInfo(memInfo);

            dic = new Dictionary<string, string>
            {
                { "TotalMemoryMB", (totalMemory / 1024 / 1024).ToString() },
                { "FreeMemoryMB", (freeMemory / 1024 / 1024).ToString() },
                { "UsedMemoryMB", (usedMemory / 1024 / 1024).ToString() },
                { "MaxMemoryMB", (maxMemory / 1024 / 1024).ToString() },
                { "SystemLowMemory", memInfo.LowMemory.ToString() },
                { "SystemAvailableMB", (memInfo.AvailMem / 1024 / 1024).ToString() },
                { "SystemTotalMB", (memInfo.TotalMem / 1024 / 1024).ToString() }
            };
#else
            dic= new Dictionary<string, string> { { "Error", "Non-Android platform" } };
#endif
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[DiagnosticsCollector] Error obteniendo memoria: {ex.Message}");
            dic = new Dictionary<string, string> { { "Error", ex.Message } };
        }

        return dic;
    }

    public string GetMemoryInfoText()
    {
        var sb = new System.Text.StringBuilder();
        var memInfo = GetMemoryInfo();
        foreach (var kvp in memInfo)
        {
            sb.AppendLine($"{kvp.Key}: {kvp.Value}");
        }
        return sb.ToString();
    }

    public Dictionary<string, string> GetDeviceInfo()
    {
        var dic = new Dictionary<string, string>
            {
                { "Model", DeviceInfo.Model},
                { "Manufacturer", DeviceInfo.Manufacturer },
                { "OSVersion", DeviceInfo.VersionString },
                { "DeviceName", DeviceInfo.DeviceType.ToString() },
                { "Device", DeviceInfo.Platform.ToString() },
            };
        return dic;
    }

    public string GetDeviceInfoText()
    {
        var sb = new System.Text.StringBuilder();
        var deviceInfo = GetDeviceInfo();
        foreach (var kvp in deviceInfo)
        {
            sb.AppendLine($"{kvp.Key}: {kvp.Value}");
        }
        return sb.ToString();
    }


    /// <summary>
    /// Obtiene información detallada de CPU y carga del sistema
    /// </summary>
    public Dictionary<string, string> GetProcessorInfo()
    {
        try
        {
#if ANDROID
            var runtime = Java.Lang.Runtime.GetRuntime();
            var processors = System.Environment.ProcessorCount;
            var availableProcessors = runtime.AvailableProcessors();
            var usageProcessor = System.Environment.CpuUsage;

            var info = new Dictionary<string, string>
            {
                { "AvailableProcessors", availableProcessors.ToString() },
                { "ProcessorCount", processors.ToString() },
                { "UsageProcessor", usageProcessor.ToString()??"" }
            };

            return info;
#else
            return new Dictionary<string, string> { { "Error", "Non-Android platform" } };
#endif
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[DiagnosticsCollector] Error obteniendo carga del sistema: {ex.Message}");
            return new Dictionary<string, string> { { "Error", ex.Message } };
        }
    }

    public string GetProcessorInfoText()
    {
        var sb = new System.Text.StringBuilder();
        var deviceInfo = GetProcessorInfo();
        foreach (var kvp in deviceInfo)
        {
            sb.AppendLine($"{kvp.Key}: {kvp.Value}");
        }
        return sb.ToString();
    }

    public Dictionary<string, string> GetRunningProcesses()
    {
        var dic = new Dictionary<string, string>();
        try
        {
#if ANDROID
            var processInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "/system/bin/ps",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            using (var process = System.Diagnostics.Process.Start(processInfo))
            {
                if (process != null)
                {
                    var output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    var lines = output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    var processList = new List<string>();
                    var processCount = 0;

                    // Procesar líneas saltando el encabezado
                    foreach (var line in lines.Skip(1).Take(50))
                    {
                        try
                        {
                            var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                            // Formato típico de ps: USER PID PPID VSIZE RSS WCHAN PC NAME
                            // Extraer PID (columna 1) y NAME (última columna)
                            if (parts.Length >= 2)
                            {
                                var pid = parts[1];
                                var processName = parts[^1]; // Última columna

                                processList.Add($"[PID: {pid}] {processName}");
                                processCount++;
                            }
                        }
                        catch
                        {
                            // Ignorar líneas que no se puedan parsear
                        }
                    }

                    if (processList.Count > 0)
                    {
                        dic.Add("RunningProcesses", string.Join(" | ", processList));
                        dic.Add("ProcessCount", processList.Count.ToString());
                    }
                    else
                    {
                        dic.Add("Error", "No se encontraron procesos");
                    }
                }
            }
#else
            dic = new Dictionary<string, string> { { "Error", "Non-Android platform" } };
#endif
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error obteniendo procesos: {ex.Message}");
            dic = new Dictionary<string, string> { { "Error", ex.Message } };
        }

        return dic;
    }

    public string GetRunningProcessesText()
    {
        var sb = new System.Text.StringBuilder();
        var processes = GetRunningProcesses();
        foreach (var kvp in processes)
        {
            sb.AppendLine($"{kvp.Key}: {kvp.Value}");
        }
        return sb.ToString();
    }

    public string GetContentFileLog()
    {
        string lineas = _logReaderService.ReadLogs(600);
    
        return lineas;
    }

    public void ClearFileLog()
    {
        _logReaderService.ClearLogs();
    }


    public string GetContentLogcat()
    {
        return _soLoggerService.GetLogsByLevel(LogcatLogLevel.ALL, 600);
    }
}

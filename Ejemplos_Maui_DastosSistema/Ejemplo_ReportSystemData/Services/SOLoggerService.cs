
using Ejemplo_ReportSystemData.Utilities;

namespace Ejemplo_ReportSystemData.Services;

public class SOLoggerService
{
    public string GetLogs()
    {
        string Logcat=string.Empty;

#if ANDROID
        int pid = Android.OS.Process.MyPid();
        //return "Logger for Android platform.";

        // Obtiene el locat de android
        // "logcat -d" vuelca el contenido del log actual y se detiene
        // "tag:V" filtraría por una etiqueta específica si la usas
        var proceso = Java.Lang.Runtime.GetRuntime().Exec($"logcat -d --pid {pid}");
        var reader = new Java.IO.BufferedReader(new Java.IO.InputStreamReader(proceso.InputStream));

        var logCompleto = new System.Text.StringBuilder();
        string linea;
        while ((linea = reader.ReadLine()) != null)
        {
            logCompleto.AppendLine(linea);
        }
        Logcat = logCompleto.ToString();
#elif IOS
            Logcat = "no disponible";
#endif
        return Logcat;
    }

    //Obtener logs por nivel específico
    public string GetLogsByLevel(LogcatLogLevel level, int maxLines = 200)
    {
        string logcat = string.Empty;

#if ANDROID
        try
        {
            int pid = Android.OS.Process.MyPid();

            // Mapear nivel a formato logcat
            string levelFilter = level switch
            {
                LogcatLogLevel.Verbose => "*:V",
                LogcatLogLevel.Debug => "*:D",
                LogcatLogLevel.Info => "*:I",
                LogcatLogLevel.Warning => "*:W",
                LogcatLogLevel.Error => "*:E",
                LogcatLogLevel.Fatal => "*:F",
                LogcatLogLevel.ALL => "",
                _ => ""
            };

            var comando = $"logcat -d -t {maxLines} --pid={pid} {levelFilter}";

            var proceso = Java.Lang.Runtime.GetRuntime()?.Exec(comando);
            if (proceso == null)
                return "[Error: No se pudo ejecutar logcat]";

            using var reader = new Java.IO.BufferedReader(
                new Java.IO.InputStreamReader(proceso.InputStream));

            var logCompleto = new System.Text.StringBuilder();
            string? linea;

            while ((linea = reader.ReadLine()) != null)
            {
                logCompleto.AppendLine(linea);
            }

            logcat = logCompleto.ToString();
        }
        catch (Exception ex)
        {
            logcat = $"[Error obteniendo logs de nivel {level}: {ex.Message}]";
        }

#elif IOS
        logcat = "[Logcat no disponible en iOS]";
#else
        logcat = "[Logcat solo disponible en Android]";
#endif

        return logcat;
    }

    //Obtener logs por rango de tiempo
    public string GetRecentLogs(TimeSpan timeSpan, int maxLines = 500)
    {
        string logcat = string.Empty;

#if ANDROID
        try
        {
            int pid = Android.OS.Process.MyPid();

            // Calcular tiempo en formato logcat (ej: "12-28 14:30:00.000")
            var timeAgo = DateTime.Now.Subtract(timeSpan);
            var timeStr = timeAgo.ToString("MM-dd HH:mm:ss.fff");

            // -t: tiempo desde
            var comando = $"logcat -d -t '{timeStr}' --pid={pid}";

            var proceso = Java.Lang.Runtime.GetRuntime()?.Exec(comando);
            if (proceso == null)
                return "[Error: No se pudo ejecutar logcat]";

            using var reader = new Java.IO.BufferedReader(
                new Java.IO.InputStreamReader(proceso.InputStream));

            var logCompleto = new System.Text.StringBuilder();
            string? linea;
            int count = 0;

            while ((linea = reader.ReadLine()) != null && count < maxLines)
            {
                logCompleto.AppendLine(linea);
                count++;
            }

            logcat = logCompleto.ToString();

            System.Diagnostics.Debug.WriteLine($"[SOLogger] Obtenidos {count} logs de últimos {timeSpan.TotalMinutes} minutos");
        }
        catch (Exception ex)
        {
            logcat = $"[Error obteniendo logs recientes: {ex.Message}]";
        }

#elif IOS
        logcat = "[Logcat no disponible en iOS]";
#else
        logcat = "[Logcat solo disponible en Android]";
#endif

        return logcat;
    }
}


namespace Ejemplo_ReportSystemData.Services;

public class SOLoggerService
{
    public string GetLoggerInfo()
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
}

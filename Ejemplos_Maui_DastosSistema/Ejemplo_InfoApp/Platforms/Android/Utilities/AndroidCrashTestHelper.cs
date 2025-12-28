
#if ANDROID
using Java.Interop;
using Java.Lang;

namespace Ejemplo_InfoApp.Platforms.Android.Utilities;

public static class AndroidCrashTestHelper
{
    /// <summary>
    /// Genera un crash nativo de Android mediante excepción no capturada en el hilo principal
    /// </summary>
    public static void TriggerAndroidMainThreadCrash()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            // Excepción que no será capturada por los handlers normales
            throw new RuntimeException("Crash de prueba: Excepción en Main Thread de Android");
        });
    }

    /// <summary>
    /// Genera un crash mediante JNI (Java Native Interface)
    /// Simula fallos de bajo nivel que cierran la app sin posibilidad de recuperación
    /// </summary>
    public static void TriggerJniNativeCrash()
    {
        try
        {
            // Intentar llamar a un método Java que no existe
            var activity = Platform.CurrentActivity;
            var method = activity.Class.GetMethod("nonExistentMethod");
            method?.Invoke(activity); // ❌ Crash por NullPointerException en Android
        }
        catch (Java.Lang.Exception ex)
        {
            throw new JavaException("Crash JNI: " + ex.Message);
        }
    }

    /// <summary>
    /// Genera un crash de segmentación simulado mediante acceso a memoria inválida
    /// </summary>
    public static void TriggerSegmentationFaultSimulation()
    {
        // Simulación de fallo de memoria mediante excepción de tipo bajo nivel
        throw new AccessViolationException("Crash de prueba: Violación de acceso a memoria");
    }
}
#endif


namespace Ejemplo_InfoApp.Utilites;

public class CrashTestHelper
{
    /// <summary>
    /// Genera un crash no controlado a nivel MAUI
    /// Será capturado por AppDomain.UnhandledException
    /// </summary>
    public static void TriggerMauiUnhandledExceptionCrash()
    {
        // Fuerza un NullReferenceException que no será manejado
        string nullString = null;
        int length = nullString.Length; // ❌ Crash aquí
    }

    /// <summary>
    /// Genera un crash en una Task que no es observada
    /// Será capturado por TaskScheduler.UnobservedTaskException
    /// </summary>
    public static void TriggerUnobservedTaskExceptionCrash()
    {
        // Crear una tarea que lanzará una excepción no observada
        Task.Run(() =>
        {
            throw new InvalidOperationException(
                "Crash de prueba: Excepción no observada en Task");
        });

        // Forzar recolección de basura para que se lance la excepción
        GC.Collect();
        GC.WaitForPendingFinalizers();
    }

    /// <summary>
    /// Genera un StackOverflowException mediante recursión infinita
    /// </summary>
    public static void TriggerStackOverflowCrash()
    {
        RecurseInfinitely();
    }

    private static void RecurseInfinitely()
    {
        RecurseInfinitely(); // Recursión sin base case
    }

    /// <summary>
    /// Genera un OutOfMemoryException asignando memoria excesiva
    /// </summary>
    public static void TriggerOutOfMemoryCrash()
    {
        var bigList = new List<byte[]>();
        try
        {
            while (true)
            {
                bigList.Add(new byte[1024 * 1024]); // 1 MB cada iteración
            }
        }
        catch
        {
            throw new OutOfMemoryException("Crash de prueba: Memoria agotada");
        }
    }
}

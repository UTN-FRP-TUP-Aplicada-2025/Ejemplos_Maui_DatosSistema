using System;
using System.Collections.Generic;
using System.Text;

namespace Ejemplo_ReportSystemData.Platforms.Android.Utilities;
#if ANDROID
internal class AndroidSIGKILLTestHelper
{

    //Forzar SIGKILL por ANR (Bloqueo de Hilo Principal)
    public static void TriggerAndroidMainThreadCrash()
    {
        // Usamos el prefijo global:: para forzar la búsqueda en las librerías del SDK de Android
        var mainLooper = global::Android.OS.Looper.MainLooper;
        var mainHandler = new global::Android.OS.Handler(mainLooper);

        mainHandler.Post(() =>
        {
            while (true)
            {
                global::System.Threading.Thread.Sleep(1000);
            }
        });
    }

    //Forzar SIGKILL por LMK (Low Memory Killer)
    //agotar la memoria del sistema tan rápido que el recolector de basura de .NET no tenga tiempo de lanzar la excepción OutOfMemo
    public static void TriggerNativeMemorySaturation()
    {
        Task.Run(() =>
        {
            var listaNativa = new List<IntPtr>();
            try
            {
                while (true)
                {
                    // Asignamos memoria nativa (fuera del control del GC de .NET)
                    // Esto es mucho más agresivo y suele terminar en SIGKILL
                    IntPtr pointer = System.Runtime.InteropServices.Marshal.AllocHGlobal(1024 * 1024 * 100); // 100MB
                    listaNativa.Add(pointer);
                }
            }
            catch { /* El sistema matará la app antes de llegar aquí */ }
        });
    }

    //read Deadlock" (Abrazo mortal)
    public static void TriggerDeadlockCrash()
    {
        var lock1 = new object();
        var lock2 = new object();

        var t1 = Task.Run(() => {
            lock (lock1)
            {
                Thread.Sleep(100);
                lock (lock2) { }
            }
        });

        lock (lock2)
        {
            Thread.Sleep(100);
            lock (lock1) { } // ❌ Deadlock eterno: El hilo principal muere aquí.
        }
    }
}
#endif
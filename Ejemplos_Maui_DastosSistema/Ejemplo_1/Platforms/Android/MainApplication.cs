using Android.App;
using Android.Runtime;
using Ejemplo_1.Services;

namespace Ejemplo_1;

[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    public override void OnCreate()
    {
        base.OnCreate();

        // Capturar excepciones de Android
        AndroidEnvironment.UnhandledExceptionRaiser += (sender, e) =>
        {
            try
            {
                var exception = e.Exception;
                FileLoggerService.Instance?.LogException(
                    exception,
                    "Excepción no manejada en AndroidEnvironment"
                );
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error logging exception: {ex.Message}");
            }

            // Marcar como manejado para evitar crash
            e.Handled = true;
        };
    }
}

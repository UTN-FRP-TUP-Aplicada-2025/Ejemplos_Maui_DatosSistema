using Android.App;
using Android.Runtime;

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

        AndroidEnvironment.UnhandledExceptionRaiser += (sender, e) =>
        {
            try
            {
                //var crashReporter = new CrashReporterService();
               // crashReporter.SendAsync(e.Exception);
            }
            catch
            {
                // nunca lanzar nada acá
            }

            // true = Android considera que ya lo manejaste
            e.Handled = true;
        };
    }
}

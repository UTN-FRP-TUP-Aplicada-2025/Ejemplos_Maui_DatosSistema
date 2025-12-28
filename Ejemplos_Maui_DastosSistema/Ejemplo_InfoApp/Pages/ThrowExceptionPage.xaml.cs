namespace Ejemplo_InfoApp.Pages;



#if ANDROID
using Ejemplo_InfoApp.Services;
using Ejemplo_InfoApp.Utilites;

using Ejemplo_InfoApp.Platforms.Android.Utilities;
#endif

public partial class ThrowExceptionPage : ContentPage
{
    private string _logs = string.Empty;

    public string Logs
    {
        get => _logs;
        set
        {
            if (_logs != value)
            {
                _logs = value;
                OnPropertyChanged();
            }
        }
    }
    
    public ThrowExceptionPage()
	{
		InitializeComponent();
        BindingContext = this;
    }
   
#if ANDROID
    // Crash a nivel MAUI
    private void OnMauiCrashClicked(object sender, EventArgs e)
    {
        CrashTestHelper.TriggerMauiUnhandledExceptionCrash();
    }

    // Crash en Task no observada
    private void OnUnobservedTaskCrashClicked(object sender, EventArgs e)
    {
        CrashTestHelper.TriggerUnobservedTaskExceptionCrash();
    }

    // Stack Overflow
    private void OnStackOverflowClicked(object sender, EventArgs e)
    {
        CrashTestHelper.TriggerStackOverflowCrash();
    }

    // Out of Memory
    private void OnOutOfMemoryClicked(object sender, EventArgs e)
    {
        CrashTestHelper.TriggerOutOfMemoryCrash();
    }


    // Crashes específicos de Android
    private void OnAndroidMainThreadCrashClicked(object sender, EventArgs e)
    {
        //AndroidCrashTestHelper.TriggerAndroidMainThreadCrash();
    }

    private void OnJniCrashClicked(object sender, EventArgs e)
    {
       // AndroidCrashTestHelper.TriggerJniNativeCrash();
    }

    private void OnSegfaultClicked(object sender, EventArgs e)
    {
        AndroidCrashTestHelper.TriggerSegmentationFaultSimulation();
    }
#else
    private void OnMauiCrashClicked(object sender, EventArgs e) { }
    private void OnUnobservedTaskCrashClicked(object sender, EventArgs e) { }
    private void OnStackOverflowClicked(object sender, EventArgs e) { }

    private void OnAndroidMainThreadCrashClicked(object sender, EventArgs e)
    {
        
    }

    private void OnJniCrashClicked(object sender, EventArgs e)
    {
       
    }

    private void OnSegfaultClicked(object sender, EventArgs e)
    {
        
    }
#endif

    private void BtnLogs_Clicked(object sender, EventArgs e)
    {
#if ANDROID
        var diagnostics = FileLoggerService.Instance?.GetLogDiagnostics() ?? "Servicio NULL";
        var logs = FileLoggerService.Instance?.GetLogContent() ?? "No hay logs";

        // Mostrar tanto diagnósticos como logs
        Logs = $"{diagnostics}\n\n=== CONTENIDO DEL LOG ===\n{logs}";
#endif
    }
}
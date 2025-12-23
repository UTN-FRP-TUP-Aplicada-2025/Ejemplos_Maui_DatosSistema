namespace Ejemplo_1.Pages;

#if ANDROID
using Ejemplo_1.Platforms.Android.Utilities;
using Ejemplo_1.Services;
using Ejemplo_1.Utilites;
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
        AndroidCrashTestHelper.TriggerAndroidMainThreadCrash();
    }

    private void OnJniCrashClicked(object sender, EventArgs e)
    {
        AndroidCrashTestHelper.TriggerJniNativeCrash();
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
        var logs = FileLoggerService.Instance?.GetLogContent();
        Logs = logs ?? "No hay logs disponibles";
#endif
    }
}
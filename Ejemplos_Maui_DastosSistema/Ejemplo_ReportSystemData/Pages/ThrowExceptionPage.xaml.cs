#if ANDROID
using Ejemplo_ReportSystemData.Platforms.Android.Utilities;
#endif
using Ejemplo_ReportSystemData.Utilites;

namespace Ejemplo_ReportSystemData.Pages;

public partial class ThrowExceptionPage : ContentPage
{
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

    
}
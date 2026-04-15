#if ANDROID
using Ejemplo_ReportSystemData.Platforms.Android.Utilities;
#endif
using Ejemplo_ReportSystemData.Utilites;

namespace Ejemplo_ReportSystemData.Pages;

public partial class ThrowExceptionPage : ContentPage
{
    public ThrowExceptionPage()
    {
        InitializeComponent();
        BindingContext = this;
    }


    // Crash a nivel MAUI
    private void OnMauiCrashClicked(object sender, EventArgs e)
    {
#if ANDROID
        CrashTestHelper.TriggerMauiUnhandledExceptionCrash();
#endif
    }

    // Crash en Task no observada
    private void OnUnobservedTaskCrashClicked(object sender, EventArgs e)
    {
#if ANDROID
        CrashTestHelper.TriggerUnobservedTaskExceptionCrash();
#endif
    }

    // Stack Overflow
    private void OnStackOverflowClicked(object sender, EventArgs e)
    {
#if ANDROID
        CrashTestHelper.TriggerStackOverflowCrash();
#endif
    }

    // Out of Memory
    private void OnOutOfMemoryClicked(object sender, EventArgs e)
    {
#if ANDROID
        CrashTestHelper.TriggerOutOfMemoryCrash();
#endif
    }


    // Crashes específicos de Android
    private void OnAndroidMainThreadCrashClicked(object sender, EventArgs e)
    {
#if ANDROID
        AndroidCrashTestHelper.TriggerAndroidMainThreadCrash();
#endif
    }

    private void OnJniCrashClicked(object sender, EventArgs e)
    {
#if ANDROID
        AndroidCrashTestHelper.TriggerJniNativeCrash();
#endif
    }

    private void OnSegfaultClicked(object sender, EventArgs e)
    {
#if ANDROID
        AndroidCrashTestHelper.TriggerSegmentationFaultSimulation();
#endif
    }

    private void OnTriggerAndroidMainThreadCrashClicked(object sender, EventArgs e)
    {
#if ANDROID
        AndroidSIGKILLTestHelper.TriggerAndroidMainThreadCrash();
#endif
    }

    private void OnTriggerNativeMemorySaturationClicked(object sender, EventArgs e)
    {
#if ANDROID
        AndroidSIGKILLTestHelper.TriggerNativeMemorySaturation();
#endif
    }

    private void OnTriggerDeadlockCrashClicked(object sender, EventArgs e)
    {
#if ANDROID
        AndroidSIGKILLTestHelper.TriggerDeadlockCrash();
#endif
    }


    async private void OnVolverClicked(object? sender, EventArgs e)
    {
        //exception: await Shell.Current.GoToAsync(nameof(Pages.MainPage));

        await Shell.Current.GoToAsync("///MainPage");
        //await Shell.Current.GoToAsync("..");
    }
}
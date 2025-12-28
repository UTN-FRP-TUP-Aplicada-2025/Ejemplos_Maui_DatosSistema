using Ejemplo_InfoApp.Services;

namespace Ejemplo_InfoApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        //AppDomain.CurrentDomain.FirstChanceException
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }

    protected override void OnStart()
    {
        //cuando la aplicación inicia por primera vez
        base.OnStart();
    }

    protected override void OnResume()
    {
        //cuando la aplicación vuelve a primer plano
        base.OnResume();
    }

    protected override void CleanUp()
    {
        base.CleanUp();
    }

    protected override void OnSleep()
    {
        //cuando la aplicación pasa a segundo plano
        base.OnSleep();
    }

    public override void ActivateWindow(Window window)
    {
        base.ActivateWindow(window);
    }

    public override void CloseWindow(Window window)
    {
        base.CloseWindow(window);
    }

    private async void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        //manejo de excepciones no controladas
        var ex = e.ExceptionObject as Exception;
        FileLoggerService.Instance?.LogException((Exception)e.ExceptionObject,
            "Excepción no manejada en AppDomain"
        );
    }

    private async void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
    {
        FileLoggerService.Instance?.LogException(e.Exception,
               "Excepción no observada en Task"
           );

        e.SetObserved();
    }
}
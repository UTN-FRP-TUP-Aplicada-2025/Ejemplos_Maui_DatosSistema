using Ejemplo_1.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Ejemplo_1;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

        //// Manejador de excepciones no controladas en MAUI
        //AppDomain.CurrentDomain.UnhandledException += (s, e) =>
        //{
        //    FileLoggerService.Instance?.LogException(
        //        (Exception)e.ExceptionObject,
        //        "Excepción no manejada en AppDomain"
        //    );
        //};

        //// Manejador de Task exceptions no observadas
        //TaskScheduler.UnobservedTaskException += (s, e) =>
        //{
        //    FileLoggerService.Instance?.LogException(
        //        e.Exception,
        //        "Excepción no observada en Task"
        //    );
        //    e.SetObserved();
        //};

        
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }

    private async void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        var ex = e.ExceptionObject as Exception;
        FileLoggerService.Instance?.LogException(
            (Exception)e.ExceptionObject,
            "Excepción no manejada en AppDomain"
        );
    }

    private async void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
    {
        FileLoggerService.Instance?.LogException(
               e.Exception,
               "Excepción no observada en Task"
           );

        e.SetObserved();
    }
}
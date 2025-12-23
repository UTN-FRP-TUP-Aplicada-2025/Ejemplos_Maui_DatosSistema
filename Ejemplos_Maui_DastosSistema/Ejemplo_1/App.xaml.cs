using Ejemplo_1.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Ejemplo_1;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Capturar excepciones no manejadas
        AppDomain.CurrentDomain.UnhandledException += (s, e) =>{
            FileLoggerService.Instance?.LogException((Exception)e.ExceptionObject,"Excepción no manejada en AppDomain");
        };

        TaskScheduler.UnobservedTaskException += (s, e) =>{
            FileLoggerService.Instance?.LogException( e.Exception, "Excepción no observada en Task");
            e.SetObserved();
        };
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}
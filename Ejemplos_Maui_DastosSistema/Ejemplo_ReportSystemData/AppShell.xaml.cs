namespace Ejemplo_ReportSystemData;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(Pages.MainPage), typeof(Pages.MainPage));
        Routing.RegisterRoute(nameof(Pages.ThrowExceptionPage), typeof(Pages.ThrowExceptionPage));
    }
}

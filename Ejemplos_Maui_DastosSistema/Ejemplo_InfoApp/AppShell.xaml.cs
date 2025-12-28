using Ejemplo_InfoApp.Pages;

namespace Ejemplo_InfoApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Registrar rutas de navegación
        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        Routing.RegisterRoute(nameof(ThrowExceptionPage), typeof(ThrowExceptionPage));
    }
}

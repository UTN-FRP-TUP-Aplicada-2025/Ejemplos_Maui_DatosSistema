using Ejemplo_1.Pages;

namespace Ejemplo_1
{
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
}

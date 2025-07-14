using GymManagement.Presentation.MobileApp.Pages;

namespace GymManagement.Presentation.MobileApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("LoginPage", typeof(LoginPage));
            Routing.RegisterRoute("RegisterPage", typeof(RegisterPage));
            Routing.RegisterRoute("MainPage", typeof(MainPage));
        }
    }
}

using GymManagement.Presentation.MobileApp.Pages;

namespace GymManagement.Presentation.MobileApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(PageNames.LoginPage, typeof(LoginPage));
            Routing.RegisterRoute(PageNames.RegisterPage, typeof(RegisterPage));
            Routing.RegisterRoute(PageNames.MainPage, typeof(MainPage));
            Routing.RegisterRoute(PageNames.ProfilePage, typeof(ProfilePage));
        }
    }
}

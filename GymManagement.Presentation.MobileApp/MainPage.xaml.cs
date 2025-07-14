using GymManagement.Shared.Core.AuthManager;

namespace GymManagement.Presentation.MobileApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage(IPreferencesService preferencesService, IAuthManager authManager, AuthenticatedUser authenticatedUser)
        {
            InitializeComponent();

            Dispatcher.Dispatch(async () =>
            {
                var isLoaded = preferencesService.LoadUserFromPreferences();
                if (isLoaded)
                {
                    if (authenticatedUser.IsTokenExpired)
                    {
                        await Task.Delay(10000); // Wait for 10 seconds to ensure the auth api is launched
                        var refreshTokenResult = await authManager.RefreshToken();
                        if (!refreshTokenResult.Success)
                        {
                            authenticatedUser = new AuthenticatedUser();
                            authManager.RedirectToLogin(true);
                        }
                    }
                }
                else
                {
                    authManager.RedirectToLogin(true);
                }
            });
        }

        private void OnLogoutClicked(object sender, EventArgs e)
        {
            var authManager = App.Services.GetRequiredService<IAuthManager>();
            authManager.Logout();
        }
    }
}

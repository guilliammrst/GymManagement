using GymManagement.Presentation.MobileApp.ApiClients;
using GymManagement.Presentation.MobileApp.Pages;
using GymManagement.Presentation.MobileApp.Services;
using GymManagement.Shared.Core.AuthManager;
using GymManagement.Shared.Core.Enums;

namespace GymManagement.Presentation.MobileApp
{
    public partial class MainPage : ContentPage
    {
        private readonly GymApiClient _gymApiClient;
        private readonly AuthenticatedUser _authenticatedUser;
        private IDispatcherTimer _timer;

        public MainPage(IPreferencesService preferencesService, IAuthManager authManager, AuthenticatedUser authenticatedUser, GymApiClient gymApiClient)
        {
            _gymApiClient = gymApiClient;
            _authenticatedUser = authenticatedUser;

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

            _timer = Dispatcher.CreateTimer();
            _timer.Interval = TimeSpan.FromSeconds(10);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (QrCodeWebView.Source == null)
                QrCodeLabel.IsVisible = false;

            _timer.Tick += Timer_Tick;
            _timer.Start();

            await LoadQrCodeAsync();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            _timer.Tick -= Timer_Tick;
            _timer.Stop();
        }

        private async void Timer_Tick(object sender, EventArgs e)
        {
            await LoadQrCodeAsync();
        }
        private async Task LoadQrCodeAsync()
        {
            if (_authenticatedUser.IsTokenExpired)
                return;
            try
            {
                var svgContentResult = await _gymApiClient.GetQrCodeSvgAsync();
                if (!svgContentResult.Success)
                    return;

                string svgContent = svgContentResult.Results;

                string html = $@"
                <html>
                <head>
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
                </head>
                <body style=""margin:0; padding:0; display:flex; justify-content:center; align-items:center;"">
                    {svgContent}
                </body>
                </html>";

                QrCodeLabel.IsVisible = true;
                QrCodeWebView.Source = new HtmlWebViewSource { Html = html };
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erreur", $"Impossible de charger le QR code: {ex.Message}", "OK");
            }
        }

        private async void OnSubscriptionClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(PageNames.MembershipsPage);
        }

        private async void OnProfileClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(PageNames.ProfilePage);
        }

        private void OnLogoutClicked(object sender, EventArgs e)
        {
            var authManager = App.Services.GetRequiredService<IAuthManager>();
            authManager.Logout();
        }
    }
}

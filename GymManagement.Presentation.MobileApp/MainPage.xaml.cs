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
        private bool _qrCodeLoaded = false;

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
                {
                    // Si pas de QR code disponible, masquer la section
                    QrCodeFrame.IsVisible = false;
                    _qrCodeLoaded = false;
                    return;
                }

                string svgContent = svgContentResult.Results;

                // Vérifier que le contenu SVG n'est pas vide
                if (string.IsNullOrWhiteSpace(svgContent))
                {
                    QrCodeFrame.IsVisible = false;
                    _qrCodeLoaded = false;
                    return;
                }

                string html = $@"
                <html>
                <head>
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
                    <style>
                        body {{
                            margin: 0;
                            padding: 20px;
                            display: flex;
                            justify-content: center;
                            align-items: center;
                            background-color: transparent;
                            font-family: 'OpenSans', sans-serif;
                        }}
                        svg {{
                            max-width: 100%;
                            height: auto;
                            border-radius: 12px;
                            background-color: white;
                            padding: 8px;
                        }}
                    </style>
                </head>
                <body>
                    {svgContent}
                </body>
                </html>";

                QrCodeWebView.Source = new HtmlWebViewSource { Html = html };
                
                // Afficher la section QR code maintenant qu'il est chargé
                if (!_qrCodeLoaded)
                {
                    QrCodeFrame.IsVisible = true;
                    _qrCodeLoaded = true;
                }
            }
            catch (Exception ex)
            {
                // En cas d'erreur, masquer la section QR code
                QrCodeFrame.IsVisible = false;
                _qrCodeLoaded = false;
                
                // Optionnel: afficher l'erreur seulement en debug
                #if DEBUG
                await DisplayAlert("Erreur", $"Impossible de charger le QR code: {ex.Message}", "OK");
                #endif
            }
        }

        private async void OnSubscriptionClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(PageNames.MembershipsPage);
        }

        private async void OnCoachingClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(PageNames.CoachingsPage);
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

using GymManagement.Shared.Core.AuthManager;

namespace GymManagement.Presentation.MobileApp.Pages
{
    public partial class LoginPage : ContentPage
    {
        private readonly IAuthManager _authManager;

        private LoginPage(IAuthManager authManager)
        {
            _authManager = authManager;

            InitializeComponent();
        }

        public LoginPage() : this(App.Services.GetRequiredService<IAuthManager>())
        {
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            var loginDto = new LoginDto
            {
                Email = emailEntry.Text,
                Password = passwordEntry.Text
            };

            var loginResult = await _authManager.Login(loginDto);

            if (loginResult.Success)
            {
                await DisplayAlert("Succès", "Connexion réussie", "OK");
                
                _authManager.RedirectToHome();
            }
            else
            {
                await DisplayAlert("Erreur", "Identifiants incorrects.", "OK");
            }
        }

        private async void OnGoToRegisterClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(PageNames.RegisterPage);
        }
    }
}

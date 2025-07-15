using GymManagement.Shared.Core.AuthManager;
using GymManagement.Shared.Core.Enums;

namespace GymManagement.Presentation.MobileApp.Pages
{
    public partial class RegisterPage : ContentPage
    {
        private readonly IAuthManager _authManager;

        private RegisterPage(IAuthManager authManager)
        {
            _authManager = authManager;

            InitializeComponent();

            genderPicker.ItemsSource = Enum.GetValues(typeof(Gender))
                                          .Cast<Gender>()
                                          .ToList();

            countryPicker.ItemsSource = Enum.GetValues(typeof(Country))
                                           .Cast<Country>()
                                           .ToList();
        }

        public RegisterPage() : this(App.Services.GetRequiredService<IAuthManager>())
        {
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            var registerDto = new RegisterDto
            {
                Name = nameEntry.Text,
                Surname = surnameEntry.Text,
                Birthdate = birthdatePicker.Date,
                Email = emailEntry.Text,
                Password = passwordEntry.Text,
                PhoneNumber = phoneEntry.Text,
                Gender = genderPicker.SelectedItem as Gender?,
                Country = countryPicker.SelectedItem as Country?,
                City = cityEntry.Text,
                Street = streetEntry.Text,
                PostalCode = postalCodeEntry.Text,
                Number = numberEntry.Text
            };

            var registerResult = await _authManager.Register(registerDto);

            if (registerResult.Success)
            {
                await DisplayAlert("Succès", "Connexion réussie", "OK");
                
                _authManager.RedirectToHome();
            }
            else
            {
                await DisplayAlert("Erreur", registerResult.ErrorMessage ?? "Erreur lors de la création du compte.", "OK");
            }
        }

        private async void OnGoToLoginClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//" + PageNames.LoginPage);
        }
    }
}

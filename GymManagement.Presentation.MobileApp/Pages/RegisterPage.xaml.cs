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
            
            // Configuration des Pickers
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
            // Validation des champs obligatoires
            if (!ValidateRequiredFields())
                return;

            try
            {
                var registerDto = new RegisterDto
                {
                    Name = nameEntry.Text?.Trim(),
                    Surname = surnameEntry.Text?.Trim(),
                    Birthdate = birthdatePicker.Date,
                    Email = emailEntry.Text?.Trim().ToLowerInvariant(),
                    Password = passwordEntry.Text,
                    PhoneNumber = phoneEntry.Text?.Trim(),
                    Gender = genderPicker.SelectedItem as Gender?,
                    Country = countryPicker.SelectedItem as Country?,
                    City = cityEntry.Text?.Trim(),
                    Street = streetEntry.Text?.Trim(),
                    PostalCode = postalCodeEntry.Text?.Trim(),
                    Number = numberEntry.Text?.Trim()
                };

                var registerResult = await _authManager.Register(registerDto);

                if (registerResult.Success)
                {
                    await DisplayAlert("🎉 Félicitations !", 
                        "Votre compte a été créé avec succès !\n\n" +
                        "Bienvenue dans la communauté GymFit ! Vous pouvez maintenant accéder à tous nos services.", 
                        "Commencer !");
                    
                    _authManager.RedirectToHome();
                }
                else
                {
                    await DisplayAlert("❌ Erreur lors de l'inscription", 
                        registerResult.ErrorMessage ?? "Une erreur s'est produite lors de la création de votre compte. Veuillez réessayer.", 
                        "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("❌ Erreur inattendue", 
                    $"Une erreur inattendue s'est produite :\n{ex.Message}\n\n" +
                    "Veuillez vérifier votre connexion internet et réessayer.", 
                    "OK");
            }
        }

        private bool ValidateRequiredFields()
        {
            var errors = new List<string>();

            // Validation des champs obligatoires
            if (string.IsNullOrWhiteSpace(nameEntry.Text))
                errors.Add("• Le nom est obligatoire");

            if (string.IsNullOrWhiteSpace(surnameEntry.Text))
                errors.Add("• Le prénom est obligatoire");

            if (string.IsNullOrWhiteSpace(emailEntry.Text))
                errors.Add("• L'email est obligatoire");
            else if (!IsValidEmail(emailEntry.Text))
                errors.Add("• L'email n'est pas valide");

            if (string.IsNullOrWhiteSpace(passwordEntry.Text))
                errors.Add("• Le mot de passe est obligatoire");
            else if (passwordEntry.Text.Length < 6)
                errors.Add("• Le mot de passe doit contenir au moins 6 caractères");

            // Validation de l'âge
            var age = DateTime.Today.Year - birthdatePicker.Date.Year;
            if (birthdatePicker.Date.Date > DateTime.Today.AddYears(-age)) age--;
            
            if (age < 16)
                errors.Add("• Vous devez avoir au moins 16 ans pour vous inscrire");
            if (age > 120)
                errors.Add("• Veuillez vérifier votre date de naissance");

            // Affichage des erreurs s'il y en a
            if (errors.Count > 0)
            {
                var errorMessage = "Veuillez corriger les erreurs suivantes :\n\n" + string.Join("\n", errors);
                DisplayAlert("⚠️ Informations manquantes", errorMessage, "OK");
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private async void OnGoToLoginClicked(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.GoToAsync("//" + PageNames.LoginPage);
            }
            catch (Exception ex)
            {
                await DisplayAlert("❌ Erreur", 
                    $"Impossible d'accéder à la page de connexion :\n{ex.Message}", 
                    "OK");
            }
        }
    }
}

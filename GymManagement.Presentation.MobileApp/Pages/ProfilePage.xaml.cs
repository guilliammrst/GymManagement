using GymManagement.Application.Interfaces.Controllers.DTOs;
using GymManagement.Presentation.MobileApp.ApiClients;
using GymManagement.Shared.Core.AuthManager;
using GymManagement.Shared.Core.Enums;

namespace GymManagement.Presentation.MobileApp.Pages
{
    public partial class ProfilePage : ContentPage
    {
        private readonly GymApiClient _gymApiClient;
        private readonly IAuthManager _authManager;
        private int UserId;

        private ProfilePage(GymApiClient gymApiClient, IAuthManager authManager)
        {
            _gymApiClient = gymApiClient;
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

        public ProfilePage() : this(App.Services.GetRequiredService<GymApiClient>(), App.Services.GetRequiredService<IAuthManager>())
        {
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadUserData();
        }

        private async Task LoadUserData()
        {
            try
            {
                var userResult = await _gymApiClient.MeAsync();
                if (userResult.Success)
                {
                    var user = userResult.Results;

                    // Informations personnelles
                    nameEntry.Text = user.Name;
                    surnameEntry.Text = user.Surname;
                    birthdatePicker.Date = user.Birthdate != default ? user.Birthdate : DateTime.Today;
                    emailEntry.Text = user.Email;
                    phoneEntry.Text = user.PhoneNumber;

                    // Sélection du genre
                    var genderIndex = Array.IndexOf(Enum.GetValues(typeof(Gender)), user.Gender);
                    if (genderIndex >= 0)
                        genderPicker.SelectedIndex = genderIndex;

                    // Adresse
                    if (user.Address != null)
                    {
                        // Sélection du pays
                        var countryIndex = Array.IndexOf(Enum.GetValues(typeof(Country)), user.Address.Country);
                        if (countryIndex >= 0)
                            countryPicker.SelectedIndex = countryIndex;

                        cityEntry.Text = user.Address.City;
                        streetEntry.Text = user.Address.Street;
                        postalCodeEntry.Text = user.Address.PostalCode;
                        numberEntry.Text = user.Address.Number;
                    }

                    UserId = user.Id;
                }
                else
                {
                    await DisplayAlert("❌ Erreur", 
                        $"Impossible de charger vos informations :\n{userResult.ErrorMessage}", 
                        "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("❌ Erreur", 
                    $"Une erreur s'est produite lors du chargement :\n{ex.Message}", 
                    "OK");
            }
        }

        private async void OnUpdateClicked(object sender, EventArgs e)
        {
            // Validation des champs obligatoires
            if (!ValidateRequiredFields())
                return;

            try
            {
                var updateDto = new UpdateUserDto
                {
                    Name = nameEntry.Text?.Trim(),
                    Surname = surnameEntry.Text?.Trim(),
                    Birthdate = birthdatePicker.Date,
                    Email = emailEntry.Text?.Trim().ToLowerInvariant(),
                    Password = string.IsNullOrWhiteSpace(passwordEntry.Text) ? null : passwordEntry.Text,
                    PhoneNumber = phoneEntry.Text?.Trim(),
                    Gender = genderPicker.SelectedIndex >= 0 ? (Gender?)genderPicker.SelectedItem : null,
                    Country = countryPicker.SelectedIndex >= 0 ? (Country?)countryPicker.SelectedItem : null,
                    City = cityEntry.Text?.Trim(),
                    Street = streetEntry.Text?.Trim(),
                    PostalCode = postalCodeEntry.Text?.Trim(),
                    Number = numberEntry.Text?.Trim()
                };

                var result = await _gymApiClient.UpdateUserAsync(UserId, updateDto);

                if (result.Success)
                {
                    // Vider le champ mot de passe après une mise à jour réussie
                    passwordEntry.Text = string.Empty;
                    
                    await DisplayAlert("🎉 Succès !", 
                        "Votre profil a été mis à jour avec succès !\n\n" +
                        "Toutes vos modifications ont été sauvegardées.", 
                        "Parfait !");
                }
                else
                {
                    await DisplayAlert("❌ Erreur de mise à jour", 
                        result.ErrorMessage ?? "Une erreur s'est produite lors de la mise à jour de votre profil. Veuillez réessayer.", 
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

            // Validation du mot de passe s'il est fourni
            if (!string.IsNullOrWhiteSpace(passwordEntry.Text) && passwordEntry.Text.Length < 6)
                errors.Add("• Le mot de passe doit contenir au moins 6 caractères");

            // Validation de l'âge
            var age = DateTime.Today.Year - birthdatePicker.Date.Year;
            if (birthdatePicker.Date.Date > DateTime.Today.AddYears(-age)) age--;
            
            if (age < 16)
                errors.Add("• Vous devez avoir au moins 16 ans");
            if (age > 120)
                errors.Add("• Veuillez vérifier votre date de naissance");

            // Affichage des erreurs s'il y en a
            if (errors.Count > 0)
            {
                var errorMessage = "Veuillez corriger les erreurs suivantes :\n\n" + string.Join("\n", errors);
                DisplayAlert("⚠️ Informations invalides", errorMessage, "OK");
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

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            // Double confirmation pour éviter les suppressions accidentelles
            var firstConfirm = await DisplayAlert("⚠️ Suppression du compte", 
                "Êtes-vous absolument certain de vouloir supprimer votre compte ?\n\n" +
                "Cette action est irréversible et vous perdrez :\n" +
                "• Tous vos abonnements\n" +
                "• Tous vos coachings\n" +
                "• Votre historique complet", 
                "Oui, supprimer", "Annuler");
            
            if (!firstConfirm)
                return;

            var secondConfirm = await DisplayAlert("🚨 Dernière confirmation", 
                "Vous êtes sur le point de supprimer définitivement votre compte.\n\n" +
                "Cette action ne peut pas être annulée !", 
                "Supprimer définitivement", "Non, garder mon compte");
            
            if (!secondConfirm)
                return;

            try
            {
                var result = await _gymApiClient.DeleteUserAsync(UserId);

                if (result.Success)
                {
                    await DisplayAlert("✅ Compte supprimé", 
                        "Votre compte a été supprimé avec succès.\n\n" +
                        "Merci d'avoir utilisé GymFit. Nous espérons vous revoir bientôt !", 
                        "Au revoir");
                    
                    _authManager.Logout();
                }
                else
                {
                    await DisplayAlert("❌ Erreur de suppression", 
                        result.ErrorMessage ?? "Une erreur s'est produite lors de la suppression de votre compte. Veuillez réessayer.", 
                        "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("❌ Erreur inattendue", 
                    $"Une erreur inattendue s'est produite :\n{ex.Message}\n\n" +
                    "Veuillez réessayer ou contacter le support.", 
                    "OK");
            }
        }

        private async void OnHomeClicked(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.GoToAsync("//" + PageNames.MainPage);
            }
            catch (Exception ex)
            {
                await DisplayAlert("❌ Erreur", 
                    $"Impossible d'accéder à l'accueil :\n{ex.Message}", 
                    "OK");
            }
        }

        private async void OnMembershipsClicked(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.GoToAsync(PageNames.MembershipsPage);
            }
            catch (Exception ex)
            {
                await DisplayAlert("❌ Erreur", 
                    $"Impossible d'accéder aux abonnements :\n{ex.Message}", 
                    "OK");
            }
        }

        private async void OnCoachingsClicked(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.GoToAsync(PageNames.CoachingsPage);
            }
            catch (Exception ex)
            {
                await DisplayAlert("❌ Erreur", 
                    $"Impossible d'accéder aux coachings :\n{ex.Message}", 
                    "OK");
            }
        }

        private void OnLogoutClicked(object sender, EventArgs e)
        {
            try
            {
                var authManager = App.Services.GetRequiredService<IAuthManager>();
                authManager.Logout();
            }
            catch (Exception ex)
            {
                DisplayAlert("❌ Erreur", 
                    $"Erreur lors de la déconnexion :\n{ex.Message}", 
                    "OK");
            }
        }
    }
}

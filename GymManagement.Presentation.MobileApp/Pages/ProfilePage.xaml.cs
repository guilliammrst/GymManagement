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
            var userResult = await _gymApiClient.MeAsync();
            if (userResult.Success)
            {
                var user = userResult.Results;

                nameEntry.Text = user.Name;
                surnameEntry.Text = user.Surname;
                birthdatePicker.Date = user.Birthdate != default ? user.Birthdate : DateTime.Today;

                emailEntry.Text = user.Email;
                phoneEntry.Text = user.PhoneNumber;

                genderPicker.SelectedIndex = (int)user.Gender;
                countryPicker.SelectedIndex = (int)user.Address.Country;

                cityEntry.Text = user.Address.City;
                streetEntry.Text = user.Address.Street;
                postalCodeEntry.Text = user.Address.PostalCode;
                numberEntry.Text = user.Address.Number;

                UserId = user.Id;
            }
        }

        private async void OnUpdateClicked(object sender, EventArgs e)
        {
            var updateDto = new UpdateUserDto
            {
                Name = nameEntry.Text,
                Surname = surnameEntry.Text,
                Birthdate = birthdatePicker.Date,
                Email = emailEntry.Text,
                Password = passwordEntry.Text,
                PhoneNumber = phoneEntry.Text,
                Gender = (Gender?)genderPicker.SelectedIndex,
                Country = (Country?)countryPicker.SelectedIndex,
                City = cityEntry.Text,
                Street = streetEntry.Text,
                PostalCode = postalCodeEntry.Text,
                Number = numberEntry.Text
            };

            var result = await _gymApiClient.UpdateUserAsync(UserId, updateDto);

            if (result.Success)
                await DisplayAlert("Succès", "Profil mis à jour", "OK");
            else
                await DisplayAlert("Erreur", result.ErrorMessage ?? "Échec de la mise à jour", "OK");
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            var result = await _gymApiClient.DeleteUserAsync(UserId);

            if (result.Success)
            {
                _authManager.Logout();
                await DisplayAlert("Succès", "Compte supprimé", "OK");
            }
            else
                await DisplayAlert("Erreur", result.ErrorMessage ?? "Échec de la suppresion du compte", "OK");
        }

        private async void OnHomeClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//" + PageNames.MainPage);
        }

        private void OnLogoutClicked(object sender, EventArgs e)
        {
            var authManager = App.Services.GetRequiredService<IAuthManager>();
            authManager.Logout();
        }
    }
}

using GymManagement.Presentation.MobileApp.ApiClients;
using GymManagement.Presentation.MobileApp.Services;
using GymManagement.Shared.Core.Enums;

namespace GymManagement.Presentation.MobileApp.Pages.CoachingFlow
{
    public partial class CoachingFlowDetailsPage : ContentPage
    {
        private readonly CoachingFlowData _coachingFlowData;
        private readonly GymApiClient _gymApiClient;

        private CoachingFlowDetailsPage(CoachingFlowData coachingFlowData, GymApiClient gymApiClient)
        {
            InitializeComponent();
            _coachingFlowData = coachingFlowData;
            _gymApiClient = gymApiClient;
            
            weekDayPicker.ItemsSource = Enum.GetValues(typeof(WeekDays))
                                    .Cast<WeekDays>()
                                    .Select(d => d.ToString())
                                    .ToList();

            hourEntry.TextChanged += HourEntry_TextChanged;
        }

        public CoachingFlowDetailsPage() : this(
            App.Services.GetRequiredService<CoachingFlowData>(),
            App.Services.GetRequiredService<GymApiClient>())
        {
        }

        private void HourEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateInputs();
        }

        private void ValidateInputs()
        {
            var isHourValid = int.TryParse(hourEntry.Text, out int hour) && hour >= 8 && hour <= 20;

            finishButton.IsEnabled = isHourValid;
        }

        private async void OnFinishClicked(object sender, EventArgs e)
        {
            if (weekDayPicker.SelectedIndex == -1 || string.IsNullOrWhiteSpace(hourEntry.Text))
            {
                await DisplayAlert("Erreur", "Veuillez remplir tous les champs.", "OK");
                return;
            }

            if (!int.TryParse(hourEntry.Text, out int parsedHour) || parsedHour < 0 || parsedHour > 23)
            {
                await DisplayAlert("Erreur", "Veuillez entrer une heure valide (0 à 23).", "OK");
                return;
            }

            _coachingFlowData.StartDate = startDatePicker.Date.AddMinutes(1439).ToUniversalTime();
            _coachingFlowData.RenewWhenExpiry = renewSwitch.IsToggled;
            _coachingFlowData.WeekDay = (WeekDays)weekDayPicker.SelectedIndex;
            _coachingFlowData.Hour = parsedHour;

            var result = await _gymApiClient.SubscribeCoachingAsync(_coachingFlowData);
            if (!result.Success)
            {
                await DisplayAlert("Erreur", "Échec de la création du coaching : " + result.ErrorMessage, "OK");
                return;
            }

            await DisplayAlert("Succès", "Coaching configuré avec succès !", "OK");

            hourEntry.TextChanged -= HourEntry_TextChanged;
            await Shell.Current.GoToAsync(PageNames.CoachingPaymentPage, new Dictionary<string, object>
            {
                { "CoachingId", result.Results.Id },
                { "UserId", _coachingFlowData.UserId }
            });
        }
    }
}

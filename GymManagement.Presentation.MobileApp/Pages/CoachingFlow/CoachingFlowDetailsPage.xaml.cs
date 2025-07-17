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
            
            weekDayPicker.ItemsSource = new List<string>
            {
                "🗓️ Lundi",
                "🗓️ Mardi", 
                "🗓️ Mercredi",
                "🗓️ Jeudi",
                "🗓️ Vendredi",
                "🗓️ Samedi",
                "🗓️ Dimanche"
            };

            hourEntry.TextChanged += HourEntry_TextChanged;
        }

        public CoachingFlowDetailsPage() : this(
            App.Services.GetRequiredService<CoachingFlowData>(),
            App.Services.GetRequiredService<GymApiClient>())
        {
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            // Initialiser avec des valeurs par défaut
            startDatePicker.Date = DateTime.Today;
            renewSwitch.IsToggled = true;
            finishButton.IsEnabled = false;
        }

        private void HourEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateInputs();
        }

        private void ValidateInputs()
        {
            var isHourValid = int.TryParse(hourEntry.Text, out int hour) && hour >= 8 && hour <= 20;
            var isDaySelected = weekDayPicker.SelectedIndex != -1;

            finishButton.IsEnabled = isHourValid && isDaySelected;
            
            // Feedback visuel pour l'heure
            if (!string.IsNullOrEmpty(hourEntry.Text))
            {
                if (isHourValid)
                {
                    hourEntry.TextColor = Colors.Green;
                }
                else
                {
                    hourEntry.TextColor = Colors.Red;
                }
            }
        }

        private async void OnFinishClicked(object sender, EventArgs e)
        {
            if (weekDayPicker.SelectedIndex == -1)
            {
                await DisplayAlert("⚠️ Attention", "Veuillez sélectionner un jour de la semaine.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(hourEntry.Text))
            {
                await DisplayAlert("⚠️ Attention", "Veuillez entrer une heure de coaching.", "OK");
                return;
            }

            if (!int.TryParse(hourEntry.Text, out int parsedHour) || parsedHour < 8 || parsedHour > 20)
            {
                await DisplayAlert("⚠️ Attention", "Veuillez entrer une heure valide entre 8 et 20.", "OK");
                return;
            }

            try
            {
                // Animation du bouton
                finishButton.IsEnabled = false;
                finishButton.Text = "⏳ Création en cours...";
                await finishButton.ScaleTo(0.95, 100);
                await finishButton.ScaleTo(1.0, 100);

                // Configuration des données
                _coachingFlowData.StartDate = startDatePicker.Date.AddMinutes(1439).ToUniversalTime();
                _coachingFlowData.RenewWhenExpiry = renewSwitch.IsToggled;
                _coachingFlowData.WeekDay = (WeekDays)weekDayPicker.SelectedIndex;
                _coachingFlowData.Hour = parsedHour;

                // Création du coaching
                var result = await _gymApiClient.SubscribeCoachingAsync(_coachingFlowData);
                if (!result.Success)
                {
                    await DisplayAlert("❌ Erreur", 
                        $"Échec de la création du coaching :\n{result.ErrorMessage}", 
                        "OK");
                    
                    // Restaurer le bouton
                    finishButton.Text = "✨ CRÉER MON COACHING";
                    finishButton.IsEnabled = true;
                    return;
                }

                // Animation de succès
                finishButton.Text = "✅ Coaching créé !";
                finishButton.BackgroundColor = Colors.Green;
                await finishButton.ScaleTo(1.1, 200);
                await finishButton.ScaleTo(1.0, 200);

                await DisplayAlert("🎉 Félicitations !", 
                    "Votre coaching a été configuré avec succès !\n\nVous allez maintenant procéder au paiement.", 
                    "Continuer");

                // Nettoyage des événements
                hourEntry.TextChanged -= HourEntry_TextChanged;

                // Navigation vers le paiement
                await Shell.Current.GoToAsync(PageNames.CoachingPaymentPage, new Dictionary<string, object>
                {
                    { "CoachingId", result.Results.Id },
                    { "UserId", _coachingFlowData.UserId }
                });
            }
            catch (Exception ex)
            {
                await DisplayAlert("❌ Erreur", $"Une erreur inattendue s'est produite :\n{ex.Message}", "OK");
                
                // Restaurer le bouton
                finishButton.Text = "✨ CRÉER MON COACHING";
                finishButton.IsEnabled = true;
                finishButton.BackgroundColor = null; // Retour au style par défaut
            }
        }
    }
}

using GymManagement.Presentation.MobileApp.ApiClients;
using GymManagement.Presentation.MobileApp.Services;
using GymManagement.Shared.Core.Enums;

namespace GymManagement.Presentation.MobileApp.Pages.SubscriptionFlow
{
    public partial class SubscriptionDetailsPage : ContentPage
    {
        private readonly SubscriptionFlowData _subscriptionFlowData;
        private readonly GymApiClient _gymApiClient;

        private SubscriptionDetailsPage(SubscriptionFlowData subscriptionFlowData, GymApiClient gymApiClient)
        {
            InitializeComponent();
            _subscriptionFlowData = subscriptionFlowData;
            _gymApiClient = gymApiClient;

            periodPicker.SelectedIndexChanged += PeriodPicker_SelectedIndexChanged;
        }

        public SubscriptionDetailsPage() : this(
            App.Services.GetRequiredService<SubscriptionFlowData>(),
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

        private void PeriodPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            finishButton.IsEnabled = periodPicker.SelectedIndex != -1;
        }

        private async void OnFinishClicked(object sender, EventArgs e)
        {
            if (periodPicker.SelectedIndex == -1)
            {
                await DisplayAlert("⚠️ Attention", "Veuillez choisir une durée d'abonnement pour continuer.", "OK");
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
                _subscriptionFlowData.StartDate = startDatePicker.Date.AddMinutes(1439).ToUniversalTime();
                _subscriptionFlowData.RenewWhenExpiry = renewSwitch.IsToggled;

                _subscriptionFlowData.MembershipPeriod = periodPicker.SelectedIndex switch
                {
                    0 => MembershipPeriod.None,
                    1 => MembershipPeriod.FourWeeks,
                    2 => MembershipPeriod.OneYear,
                    _ => MembershipPeriod.None
                };

                // Création de l'abonnement
                var result = await _gymApiClient.SubscribeUserAsync(_subscriptionFlowData);
                if (!result.Success)
                {
                    await DisplayAlert("❌ Erreur", 
                        $"Échec de la création de l'abonnement :\n{result.ErrorMessage}", 
                        "OK");
                    
                    // Restaurer le bouton
                    finishButton.Text = "✨ CRÉER MON ABONNEMENT";
                    finishButton.IsEnabled = true;
                    return;
                }

                // Animation de succès
                finishButton.Text = "✅ Abonnement créé !";
                finishButton.BackgroundColor = Colors.Green;
                await finishButton.ScaleTo(1.1, 200);
                await finishButton.ScaleTo(1.0, 200);

                await DisplayAlert("🎉 Félicitations !", 
                    "Votre abonnement a été configuré avec succès !\n\nVous allez maintenant procéder au paiement.", 
                    "Continuer");

                // Nettoyage des événements
                periodPicker.SelectedIndexChanged -= PeriodPicker_SelectedIndexChanged;

                // Navigation vers le paiement
                var latestMembership = result.Results.Memberships.OrderByDescending(m => m.Id).FirstOrDefault();
                if (latestMembership != null)
                {
                    await Shell.Current.GoToAsync(PageNames.PaymentPage, new Dictionary<string, object>
                    {
                        { "MembershipId", latestMembership.Id },
                        { "UserId", _subscriptionFlowData.UserId }
                    });
                }
                else
                {
                    await DisplayAlert("❌ Erreur", "Impossible de récupérer les informations d'abonnement.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("❌ Erreur", $"Une erreur inattendue s'est produite :\n{ex.Message}", "OK");
                
                // Restaurer le bouton
                finishButton.Text = "✨ CRÉER MON ABONNEMENT";
                finishButton.IsEnabled = true;
                finishButton.BackgroundColor = null; // Retour au style par défaut
            }
        }
    }
}

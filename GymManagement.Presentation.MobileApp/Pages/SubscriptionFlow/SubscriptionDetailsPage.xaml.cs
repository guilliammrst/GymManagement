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

        private void PeriodPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            finishButton.IsEnabled = periodPicker.SelectedIndex != -1;
        }

        private async void OnFinishClicked(object sender, EventArgs e)
        {
            if (periodPicker.SelectedIndex == -1)
            {
                await DisplayAlert("Erreur", "Veuillez choisir une durée d'abonnement.", "OK");
                return;
            }

            _subscriptionFlowData.StartDate = startDatePicker.Date.AddMinutes(1439).ToUniversalTime();
            _subscriptionFlowData.RenewWhenExpiry = renewSwitch.IsToggled;

            _subscriptionFlowData.MembershipPeriod = periodPicker.SelectedIndex switch
            {
                0 => MembershipPeriod.None,
                1 => MembershipPeriod.FourWeeks,
                2 => MembershipPeriod.OneYear,
                _ => MembershipPeriod.None
            };

            var result = await _gymApiClient.SubscribeUserAsync(_subscriptionFlowData);
            if (!result.Success)
            {
                await DisplayAlert("Erreur", "Échec de la création de l'abonnement : " + result.ErrorMessage, "OK");
                return;
            }

            await DisplayAlert("Succès", "Abonnement configuré avec succès !", "OK");

            periodPicker.SelectedIndexChanged -= PeriodPicker_SelectedIndexChanged;

            await Shell.Current.GoToAsync(PageNames.PaymentPage, new Dictionary<string, object>
            {
                { "MembershipId", result.Results.Memberships.OrderByDescending(m => m.Id).FirstOrDefault()!.Id },
                { "UserId", _subscriptionFlowData.UserId }
            }); ;
        }
    }
}

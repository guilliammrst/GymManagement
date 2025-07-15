using GymManagement.Application.Interfaces.Services.MembershipPlans;
using GymManagement.Presentation.MobileApp.ApiClients;
using GymManagement.Presentation.MobileApp.Services;

namespace GymManagement.Presentation.MobileApp.Pages.SubscriptionFlow
{
    public partial class SubscribePage : ContentPage
    {
        private readonly GymApiClient _gymApiClient;
        private readonly SubscriptionFlowData _subscriptionFlowData;
        private MembershipPlanDto? _selectedPlan;

        private SubscribePage(GymApiClient gymApiClient, SubscriptionFlowData subscriptionFlowData)
        {
            _gymApiClient = gymApiClient;
            _subscriptionFlowData = subscriptionFlowData;

            InitializeComponent();
        }

        public SubscribePage() : this(App.Services.GetRequiredService<GymApiClient>(), App.Services.GetRequiredService<SubscriptionFlowData>())
        {
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            _selectedPlan = null;
            continueButton.IsEnabled = false;
            await LoadSubscriptionData();
        }

        private async Task LoadSubscriptionData()
        {
            var membershipPlansResult = await _gymApiClient.GetMembershipPlansAsync();
            if (membershipPlansResult.Success)
            {
                plansCollectionView.ItemsSource = membershipPlansResult.Results
                    .Where(p => p.IsValid)
                    .ToList();
            }
        }

        private void OnPlanSelected(object sender, SelectionChangedEventArgs e)
        {
            _selectedPlan = e.CurrentSelection.FirstOrDefault() as MembershipPlanDto;
            continueButton.IsEnabled = _selectedPlan != null;

            if (_selectedPlan != null)
            {
                _subscriptionFlowData.MembershipPlan = _selectedPlan;
            }
        }

        private async void OnContinueClicked(object sender, EventArgs e)
        {
            if (_selectedPlan == null)
            {
                await DisplayAlert("Erreur", "Veuillez sélectionner un plan d'abonnement.", "OK");
                return;
            }

            _subscriptionFlowData.MembershipPlan = _selectedPlan;
            await Shell.Current.GoToAsync(PageNames.SubscriptionClubPage);
        }
    }
}

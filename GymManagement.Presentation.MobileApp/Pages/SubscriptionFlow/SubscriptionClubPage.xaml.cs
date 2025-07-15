using GymManagement.Application.Interfaces.Services.Clubs;
using GymManagement.Presentation.MobileApp.ApiClients;
using GymManagement.Presentation.MobileApp.Services;

namespace GymManagement.Presentation.MobileApp.Pages.SubscriptionFlow
{
    public partial class SubscriptionClubPage : ContentPage
    {
        private readonly GymApiClient _gymApiClient;
        private readonly SubscriptionFlowData _subscriptionFlowData;
        private ClubDto? _selectedClub;

        private SubscriptionClubPage(GymApiClient gymApiClient, SubscriptionFlowData subscriptionFlowData)
        {
            _gymApiClient = gymApiClient;
            _subscriptionFlowData = subscriptionFlowData;

            InitializeComponent();
        }

        public SubscriptionClubPage() : this(
            App.Services.GetRequiredService<GymApiClient>(),
            App.Services.GetRequiredService<SubscriptionFlowData>())
        {
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            _selectedClub = null;
            continueButton.IsEnabled = false;
            await LoadClubsAsync();
        }

        private async Task LoadClubsAsync()
        {
            var result = await _gymApiClient.GetClubsAsync();
            if (result.Success)
            {
                clubsCollectionView.ItemsSource = result.Results;
            }
        }

        private void OnClubSelected(object sender, SelectionChangedEventArgs e)
        {
            _selectedClub = e.CurrentSelection.FirstOrDefault() as ClubDto;
            continueButton.IsEnabled = _selectedClub != null;
            if (_selectedClub != null)
            {
                _subscriptionFlowData.HomeClub = _selectedClub;
            }
        }

        private async void OnContinueClicked(object sender, EventArgs e)
        {
            if (_selectedClub == null)
            {
                await DisplayAlert("Erreur", "Veuillez sélectionner un club.", "OK");
                return;
            }

            _subscriptionFlowData.HomeClub = _selectedClub;
            await Shell.Current.GoToAsync(PageNames.SubscriptionDetailsPage);
        }
    }
}

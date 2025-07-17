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
        private List<ClubDtoWithSelection>? _clubsWithSelection;

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
            try
            {
                var result = await _gymApiClient.GetClubsAsync();
                if (result.Success)
                {
                    _clubsWithSelection = result.Results
                        .Select(c => new ClubDtoWithSelection
                        {
                            Club = c,
                            IsSelected = false,
                            Id = c.Id,
                            Name = c.Name,
                            Address = c.Address,
                            CreatedAt = c.CreatedAt,
                            UpdatedAt = c.UpdatedAt
                        })
                        .ToList();

                    clubsCollectionView.ItemsSource = _clubsWithSelection;
                }
                else
                {
                    await DisplayAlert("❌ Erreur", "Impossible de charger la liste des clubs. Veuillez réessayer.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("❌ Erreur", $"Une erreur inattendue s'est produite : {ex.Message}", "OK");
            }
        }

        private void OnClubSelected(object sender, SelectionChangedEventArgs e)
        {
            // Désélectionner tous les clubs
            if (_clubsWithSelection != null)
            {
                foreach (var club in _clubsWithSelection)
                {
                    club.IsSelected = false;
                }
            }

            // Sélectionner le club choisi
            var selectedClubWithSelection = e.CurrentSelection.FirstOrDefault() as ClubDtoWithSelection;
            if (selectedClubWithSelection != null)
            {
                selectedClubWithSelection.IsSelected = true;
                _selectedClub = selectedClubWithSelection.Club;
                continueButton.IsEnabled = true;
                _subscriptionFlowData.HomeClub = _selectedClub;

                // Rafraîchir l'affichage
                clubsCollectionView.ItemsSource = null;
                clubsCollectionView.ItemsSource = _clubsWithSelection;
            }
            else
            {
                _selectedClub = null;
                continueButton.IsEnabled = false;
            }
        }

        private async void OnContinueClicked(object sender, EventArgs e)
        {
            if (_selectedClub == null)
            {
                await DisplayAlert("⚠️ Attention", "Veuillez sélectionner un club pour continuer.", "OK");
                return;
            }

            try
            {
                // Animation du bouton
                continueButton.IsEnabled = false;
                await continueButton.ScaleTo(0.95, 100);
                await continueButton.ScaleTo(1.0, 100);

                _subscriptionFlowData.HomeClub = _selectedClub;
                await Shell.Current.GoToAsync(PageNames.SubscriptionDetailsPage);
            }
            catch (Exception ex)
            {
                await DisplayAlert("❌ Erreur", $"Impossible de continuer : {ex.Message}", "OK");
                continueButton.IsEnabled = true;
            }
        }
    }

    // Classe helper pour gérer la sélection visuelle
    public class ClubDtoWithSelection : ClubDto
    {
        public ClubDto Club { get; set; } = new();
        public bool IsSelected { get; set; }
    }
}

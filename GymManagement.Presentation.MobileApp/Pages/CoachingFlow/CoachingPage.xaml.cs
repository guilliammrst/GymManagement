using GymManagement.Application.Interfaces.Services.CoachingPlans;
using GymManagement.Presentation.MobileApp.ApiClients;
using GymManagement.Presentation.MobileApp.Services;

namespace GymManagement.Presentation.MobileApp.Pages.CoachingFlow
{
    public partial class CoachingPage : ContentPage
    {
        private readonly GymApiClient _gymApiClient;
        private readonly CoachingFlowData _coachingFlowData;
        private CoachingPlanDto? _selectedCoachingPlan;

        private CoachingPage(GymApiClient gymApiClient, CoachingFlowData coachingFlowData)
        {
            _gymApiClient = gymApiClient;
            _coachingFlowData = coachingFlowData;

            InitializeComponent();
        }

        public CoachingPage() : this(App.Services.GetRequiredService<GymApiClient>(), App.Services.GetRequiredService<CoachingFlowData>())
        {
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            _selectedCoachingPlan = null;
            continueButton.IsEnabled = false;
            await LoadCoachingPlans();
        }

        private async Task LoadCoachingPlans()
        {
            var coachingPlansResult = await _gymApiClient.GetCoachingPlansAsync();

            if (coachingPlansResult.Success)
            {
                coachingPlansCollectionView.ItemsSource = coachingPlansResult.Results
                    .Where(p => p.IsValid)
                    .ToList();
            }
            else
            {
                await DisplayAlert("Erreur", "Impossible de charger les plans de coaching.", "OK");
            }
        }

        private void OnCoachingPlanSelected(object sender, SelectionChangedEventArgs e)
        {
            _selectedCoachingPlan = e.CurrentSelection.FirstOrDefault() as CoachingPlanDto;
            continueButton.IsEnabled = _selectedCoachingPlan != null;

            if (_selectedCoachingPlan != null)
            {
                _coachingFlowData.CoachingPlan = _selectedCoachingPlan;
            }
        }

        private async void OnContinueClicked(object sender, EventArgs e)
        {
            if (_selectedCoachingPlan == null)
            {
                await DisplayAlert("Erreur", "Veuillez sélectionner un plan de coaching.", "OK");
                return;
            }

            _coachingFlowData.CoachingPlan = _selectedCoachingPlan;
            await Shell.Current.GoToAsync(PageNames.CoachingFlowDetailsPage);
        }
    }
}

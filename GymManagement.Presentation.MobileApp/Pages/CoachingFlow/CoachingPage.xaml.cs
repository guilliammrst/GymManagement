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
        private List<CoachingPlanDtoWithSelection>? _plansWithSelection;

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
            try
            {
                var coachingPlansResult = await _gymApiClient.GetCoachingPlansAsync();

                if (coachingPlansResult.Success)
                {
                    _plansWithSelection = coachingPlansResult.Results
                        .Where(p => p.IsValid)
                        .Select(p => new CoachingPlanDtoWithSelection 
                        { 
                            Plan = p, 
                            IsSelected = false,
                            Id = p.Id,
                            Description = p.Description,
                            Price = p.Price,
                            IsValid = p.IsValid,
                            CreatedAt = p.CreatedAt,
                            UpdatedAt = p.UpdatedAt
                        })
                        .ToList();

                    coachingPlansCollectionView.ItemsSource = _plansWithSelection;
                }
                else
                {
                    await DisplayAlert("❌ Erreur", "Impossible de charger les plans de coaching. Veuillez réessayer.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("❌ Erreur", $"Une erreur inattendue s'est produite : {ex.Message}", "OK");
            }
        }

        private void OnCoachingPlanSelected(object sender, SelectionChangedEventArgs e)
        {
            // Désélectionner tous les plans
            if (_plansWithSelection != null)
            {
                foreach (var plan in _plansWithSelection)
                {
                    plan.IsSelected = false;
                }
            }

            // Sélectionner le plan choisi
            var selectedPlanWithSelection = e.CurrentSelection.FirstOrDefault() as CoachingPlanDtoWithSelection;
            if (selectedPlanWithSelection != null)
            {
                selectedPlanWithSelection.IsSelected = true;
                _selectedCoachingPlan = selectedPlanWithSelection.Plan;
                continueButton.IsEnabled = true;
                _coachingFlowData.CoachingPlan = _selectedCoachingPlan;

                // Rafraîchir l'affichage
                coachingPlansCollectionView.ItemsSource = null;
                coachingPlansCollectionView.ItemsSource = _plansWithSelection;
            }
            else
            {
                _selectedCoachingPlan = null;
                continueButton.IsEnabled = false;
            }
        }

        private async void OnContinueClicked(object sender, EventArgs e)
        {
            if (_selectedCoachingPlan == null)
            {
                await DisplayAlert("⚠️ Attention", "Veuillez sélectionner un plan de coaching pour continuer.", "OK");
                return;
            }

            try
            {
                // Animation du bouton
                continueButton.IsEnabled = false;
                await continueButton.ScaleTo(0.95, 100);
                await continueButton.ScaleTo(1.0, 100);

                _coachingFlowData.CoachingPlan = _selectedCoachingPlan;
                await Shell.Current.GoToAsync(PageNames.CoachingFlowDetailsPage);
            }
            catch (Exception ex)
            {
                await DisplayAlert("❌ Erreur", $"Impossible de continuer : {ex.Message}", "OK");
                continueButton.IsEnabled = true;
            }
        }
    }

    // Classe helper pour gérer la sélection visuelle
    public class CoachingPlanDtoWithSelection : CoachingPlanDto
    {
        public CoachingPlanDto Plan { get; set; } = new();
        public bool IsSelected { get; set; }
    }
}

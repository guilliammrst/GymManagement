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
        private List<MembershipPlanDtoWithSelection>? _plansWithSelection;

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
            try
            {
                var membershipPlansResult = await _gymApiClient.GetMembershipPlansAsync();
                if (membershipPlansResult.Success)
                {
                    _plansWithSelection = membershipPlansResult.Results
                        .Where(p => p.IsValid)
                        .Select(p => new MembershipPlanDtoWithSelection
                        {
                            Plan = p,
                            IsSelected = false,
                            Id = p.Id,
                            Description = p.Description,
                            BasePrice = p.BasePrice,
                            RegistrationFees = p.RegistrationFees,
                            YearlyDiscount = p.YearlyDiscount,
                            MembershipPlanType = p.MembershipPlanType,
                            IsValid = p.IsValid,
                            CreatedAt = p.CreatedAt,
                            UpdatedAt = p.UpdatedAt
                        })
                        .ToList();

                    plansCollectionView.ItemsSource = _plansWithSelection;
                }
                else
                {
                    await DisplayAlert("❌ Erreur", "Impossible de charger les plans d'abonnement. Veuillez réessayer.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("❌ Erreur", $"Une erreur inattendue s'est produite : {ex.Message}", "OK");
            }
        }

        private void OnPlanSelected(object sender, SelectionChangedEventArgs e)
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
            var selectedPlanWithSelection = e.CurrentSelection.FirstOrDefault() as MembershipPlanDtoWithSelection;
            if (selectedPlanWithSelection != null)
            {
                selectedPlanWithSelection.IsSelected = true;
                _selectedPlan = selectedPlanWithSelection.Plan;
                continueButton.IsEnabled = true;
                _subscriptionFlowData.MembershipPlan = _selectedPlan;

                // Rafraîchir l'affichage
                plansCollectionView.ItemsSource = null;
                plansCollectionView.ItemsSource = _plansWithSelection;
            }
            else
            {
                _selectedPlan = null;
                continueButton.IsEnabled = false;
            }
        }

        private async void OnContinueClicked(object sender, EventArgs e)
        {
            if (_selectedPlan == null)
            {
                await DisplayAlert("⚠️ Attention", "Veuillez sélectionner un plan d'abonnement pour continuer.", "OK");
                return;
            }

            try
            {
                // Animation du bouton
                continueButton.IsEnabled = false;
                await continueButton.ScaleTo(0.95, 100);
                await continueButton.ScaleTo(1.0, 100);

                _subscriptionFlowData.MembershipPlan = _selectedPlan;
                await Shell.Current.GoToAsync(PageNames.SubscriptionClubPage);
            }
            catch (Exception ex)
            {
                await DisplayAlert("❌ Erreur", $"Impossible de continuer : {ex.Message}", "OK");
                continueButton.IsEnabled = true;
            }
        }
    }

    // Classe helper pour gérer la sélection visuelle
    public class MembershipPlanDtoWithSelection : MembershipPlanDto
    {
        public MembershipPlanDto Plan { get; set; } = new();
        public bool IsSelected { get; set; }
    }
}

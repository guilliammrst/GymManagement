using GymManagement.Application.Interfaces.Services.Coachings;
using GymManagement.Presentation.MobileApp.ApiClients;

namespace GymManagement.Presentation.MobileApp.Pages
{
    [QueryProperty(nameof(Coaching), "Coaching")]
    [QueryProperty(nameof(UserId), "UserId")]
    public partial class CoachingDetailsPage : ContentPage
    {
        public CoachingDto Coaching { get; set; }
        public int UserId { get; set; }

        private readonly GymApiClient _gymApiClient;
        private CoachingDetailsDto? _coachingDetails;

        private CoachingDetailsPage(GymApiClient gymApiClient)
        {
            _gymApiClient = gymApiClient;
            InitializeComponent();
        }

        public CoachingDetailsPage() : this(App.Services.GetRequiredService<GymApiClient>())
        {
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadCoachingDetails();
        }

        private async Task LoadCoachingDetails()
        {
            try
            {
                var coachingDetailsResult = await _gymApiClient.GetCoachingByIdAsync(UserId, Coaching.Id);
                if (coachingDetailsResult.Success)
                {
                    _coachingDetails = coachingDetailsResult.Results;
                    BindingContext = _coachingDetails;
                }
                else
                {
                    await DisplayAlert("❌ Erreur",
                        $"Impossible de charger les détails du coaching :\n{coachingDetailsResult.ErrorMessage}",
                        "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("❌ Erreur",
                    $"Une erreur inattendue s'est produite :\n{ex.Message}",
                    "OK");
            }
        }

        private async void OnPayClicked(object sender, EventArgs e)
        {
            try
            {
                if (_coachingDetails == null)
                {
                    await DisplayAlert("⚠️ Attention", "Les détails du coaching ne sont pas encore chargés.", "OK");
                    return;
                }

                await Shell.Current.GoToAsync(PageNames.CoachingPaymentPage, new Dictionary<string, object>
                {
                    { "CoachingId", Coaching.Id },
                    { "UserId", UserId }
                });
            }
            catch (Exception ex)
            {
                await DisplayAlert("❌ Erreur",
                    $"Impossible d'accéder au paiement :\n{ex.Message}",
                    "OK");
            }
        }
    }
}

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
        private CoachingDetailsDto _coachingDetails;

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
            var coachingDetailsResult = await _gymApiClient.GetCoachingByIdAsync(UserId, Coaching.Id);
            if (coachingDetailsResult.Success)
            {
                _coachingDetails = coachingDetailsResult.Results;
                BindingContext = _coachingDetails;
            }
            else
            {
                await DisplayAlert("Erreur", "Impossible de charger les détails du coaching.", "OK");
            }
        }

        private async void OnPayClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(PageNames.CoachingPaymentPage, new Dictionary<string, object>
            {
                { "CoachingId", Coaching.Id },
                { "UserId", UserId }
            });
        }
    }
}

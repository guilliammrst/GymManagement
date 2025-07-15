using GymManagement.Application.Interfaces.Services.Memberships;
using GymManagement.Presentation.MobileApp.ApiClients;

namespace GymManagement.Presentation.MobileApp.Pages
{
    [QueryProperty(nameof(Subscription), "Subscription")]
    [QueryProperty(nameof(UserId), "UserId")]
    public partial class MembershipDetailsPage : ContentPage
    {
        public MembershipDto Subscription { get; set; }
        public int UserId { get; set; }
        private readonly GymApiClient _gymApiClient;
        private MembershipDetailsDto _membershipDetails;

        private MembershipDetailsPage(GymApiClient gymApiClient)
        {
            _gymApiClient = gymApiClient;

            InitializeComponent();
        }

        public MembershipDetailsPage() : this(App.Services.GetRequiredService<GymApiClient>())
        {
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadSubscriptionDetails();
        }

        private async Task LoadSubscriptionDetails()
        {
            var subscriptionDetailsResult = await _gymApiClient.GetMembershipByIdAsync(UserId, Subscription.Id);
            if (subscriptionDetailsResult.Success)
            {
                _membershipDetails = subscriptionDetailsResult.Results;
                BindingContext = _membershipDetails;
            }
            else
            {
                await DisplayAlert("Erreur", "Impossible de charger les détails de l'abonnement.", "OK");
            }
        }

        private async void OnPayClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(PageNames.PaymentPage, new Dictionary<string, object>
            {
                { "MembershipId", Subscription.Id },
                { "UserId", UserId }
            });
        }
    }
}
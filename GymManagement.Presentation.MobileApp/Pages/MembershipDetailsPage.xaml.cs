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
        private MembershipDetailsDto? _membershipDetails;

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
            try
            {
                var subscriptionDetailsResult = await _gymApiClient.GetMembershipByIdAsync(UserId, Subscription.Id);
                if (subscriptionDetailsResult.Success)
                {
                    _membershipDetails = subscriptionDetailsResult.Results;
                    BindingContext = _membershipDetails;
                }
                else
                {
                    await DisplayAlert("❌ Erreur", 
                        $"Impossible de charger les détails de l'abonnement :\n{subscriptionDetailsResult.ErrorMessage}", 
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
                if (_membershipDetails == null)
                {
                    await DisplayAlert("⚠️ Attention", "Les détails de l'abonnement ne sont pas encore chargés.", "OK");
                    return;
                }

                await Shell.Current.GoToAsync(PageNames.PaymentPage, new Dictionary<string, object>
                {
                    { "MembershipId", Subscription.Id },
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
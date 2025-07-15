using GymManagement.Application.Interfaces.Services.Memberships;
using GymManagement.Presentation.MobileApp.ApiClients;
using GymManagement.Presentation.MobileApp.Services;

namespace GymManagement.Presentation.MobileApp.Pages
{
    public partial class MembershipsPage : ContentPage
    {
        private readonly GymApiClient _gymApiClient;
        private readonly SubscriptionFlowData _subscriptionFlowData;
        private int UserId;

        private MembershipsPage(GymApiClient gymApiClient, SubscriptionFlowData subscriptionFlowData)
        {
            _gymApiClient = gymApiClient;
            _subscriptionFlowData = subscriptionFlowData;

            InitializeComponent();
        }

        public MembershipsPage() : this(App.Services.GetRequiredService<GymApiClient>(),
            App.Services.GetRequiredService<SubscriptionFlowData>())
        {
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadUserData();
        }

        private async Task LoadUserData()
        {
            var userResult = await _gymApiClient.MeAsync();
            if (userResult.Success)
            {
                var user = userResult.Results;

                if (user.Memberships != null && user.Memberships.Any())
                {
                    subscriptionsView.ItemsSource = user.Memberships;
                    emptyLabel.IsVisible = false;
                }
                else
                {
                    subscriptionsView.ItemsSource = null;
                    emptyLabel.IsVisible = true;
                }

                UserId = user.Id;
            }
        }

        private async void OnSubscribeClicked(object sender, EventArgs e)
        {
            _subscriptionFlowData.UserId = UserId;
            await Shell.Current.GoToAsync(PageNames.SubscribePage);
        }
        private async void OnSubscriptionSelected(object sender, SelectionChangedEventArgs e)
        {
            var selected = e.CurrentSelection.FirstOrDefault() as MembershipDto; 
            if (selected == null) return;

            subscriptionsView.SelectedItem = null;

            await Shell.Current.GoToAsync(PageNames.MembershipDetailsPage, new Dictionary<string, object>
            {
                { "Subscription", selected },
                { "UserId", UserId }
            });
        }
    }
}

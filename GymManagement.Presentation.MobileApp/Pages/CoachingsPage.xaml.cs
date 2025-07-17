using GymManagement.Application.Interfaces.Services.Coachings;
using GymManagement.Presentation.MobileApp.ApiClients;
using GymManagement.Presentation.MobileApp.Services;

namespace GymManagement.Presentation.MobileApp.Pages
{
    public partial class CoachingsPage : ContentPage
    {
        private readonly GymApiClient _gymApiClient;
        private readonly CoachingFlowData _coachingFlowData;
        private int UserId;

        private CoachingsPage(GymApiClient gymApiClient, CoachingFlowData coachingFlowData)
        {
            _gymApiClient = gymApiClient;
            _coachingFlowData = coachingFlowData;

            InitializeComponent();
        }

        public CoachingsPage() : this(App.Services.GetRequiredService<GymApiClient>(),
            App.Services.GetRequiredService<CoachingFlowData>())
        {
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadCoachingsData();
        }

        private async Task LoadCoachingsData()
        {
            var userResult = await _gymApiClient.MeAsync();
            if (userResult.Success)
            {
                var user = userResult.Results;

                var coachingsResult = await _gymApiClient.GetCoachingsAsync(user.Id);
                if (coachingsResult.Success)
                {
                    var coachings = coachingsResult.Results;
                    if (coachings != null && coachings.Any())
                    {
                        coachingsView.ItemsSource = coachings;
                        emptyLabel.IsVisible = false;
                    }
                    else
                    {
                        coachingsView.ItemsSource = null;
                        emptyLabel.IsVisible = true;
                    }
                }
                else
                {
                    coachingsView.ItemsSource = null;
                    emptyLabel.IsVisible = true;
                }

                UserId = user.Id;
            }
        }

        private async void OnCoachingClicked(object sender, EventArgs e)
        {
            _coachingFlowData.UserId = UserId;
            await Shell.Current.GoToAsync(PageNames.CoachingPage);
        }

        private async void OnCoachingSelected(object sender, SelectionChangedEventArgs e)
        {
            var selected = e.CurrentSelection.FirstOrDefault() as CoachingDto;
            if (selected == null) return;

            coachingsView.SelectedItem = null;

            await Shell.Current.GoToAsync(PageNames.CoachingDetailsPage, new Dictionary<string, object>
            {
                { "Coaching", selected },
                { "UserId", UserId }
            });
        }
    }
}

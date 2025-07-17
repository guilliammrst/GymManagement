using GymManagement.Application.Interfaces.Controllers.DTOs;
using GymManagement.Presentation.MobileApp.ApiClients;
using GymManagement.Shared.Core.Enums;

namespace GymManagement.Presentation.MobileApp.Pages.CoachingFlow
{
    [QueryProperty(nameof(UserId), "UserId")]
    [QueryProperty(nameof(CoachingId), "CoachingId")]
    public partial class CoachingPaymentPage : ContentPage
    {
        private readonly GymApiClient _gymApiClient;

        public int UserId { get; set; }
        public int CoachingId { get; set; }

        public List<PaymentMethod> PaymentMethods { get; } = Enum.GetValues(typeof(PaymentMethod)).Cast<PaymentMethod>().ToList();

        public CoachingPaymentPage()
        {
            InitializeComponent();
            _gymApiClient = App.Services.GetRequiredService<GymApiClient>();
            BindingContext = this;
        }

        private async void OnPayClicked(object sender, EventArgs e)
        {
            var paymentDto = new PaymentDto
            {
                PaymentMethod = (PaymentMethod)paymentMethodPicker.SelectedItem
            };

            var result = await _gymApiClient.PayCoachingAsync(UserId, CoachingId, paymentDto);

            if (result.Success)
            {
                await DisplayAlert("Succès", "Paiement effectué avec succès.", "OK");
                await Shell.Current.GoToAsync("//" + PageNames.MainPage);
            }
            else
            {
                await DisplayAlert("Erreur", "Le paiement a échoué.", "OK");
            }
        }
    }
}
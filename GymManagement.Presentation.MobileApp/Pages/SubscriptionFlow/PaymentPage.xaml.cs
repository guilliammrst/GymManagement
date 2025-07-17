using GymManagement.Application.Interfaces.Controllers.DTOs;
using GymManagement.Application.Interfaces.Services.Memberships;
using GymManagement.Presentation.MobileApp.ApiClients;
using GymManagement.Shared.Core.Enums;
using System.Globalization;

namespace GymManagement.Presentation.MobileApp.Pages.SubscriptionFlow
{
    [QueryProperty(nameof(UserId), "UserId")]
    [QueryProperty(nameof(MembershipId), "MembershipId")]
    public partial class PaymentPage : ContentPage
    {
        private readonly GymApiClient _gymApiClient;
        private MembershipDetailsDto? _membershipDetails;

        public int UserId { get; set; }
        public int MembershipId { get; set; }

        public List<string> PaymentMethods { get; } = new List<string>
        {
            "💳 Carte de crédit",
            "📱 PayPal",
            "🏦 Virement bancaire",
            "💰 Espèces",
            "💳 Autre méthode"
        };

        public PaymentPage()
        {
            InitializeComponent();
            _gymApiClient = App.Services.GetRequiredService<GymApiClient>();
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            
            // Pré-sélectionner la première méthode de paiement
            if (PaymentMethods.Count > 0)
            {
                paymentMethodPicker.SelectedIndex = 0;
            }

            // Charger les détails du membership
            await LoadMembershipDetails();
        }

        private async Task LoadMembershipDetails()
        {
            try
            {
                var membershipDetailsResult = await _gymApiClient.GetMembershipByIdAsync(UserId, MembershipId);
                if (membershipDetailsResult.Success)
                {
                    _membershipDetails = membershipDetailsResult.Results;
                    LoadSummaryData();
                }
                else
                {
                    await DisplayAlert("❌ Erreur", 
                        $"Impossible de charger les détails de l'abonnement :\n{membershipDetailsResult.ErrorMessage}", 
                        "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("❌ Erreur", 
                    $"Une erreur s'est produite lors du chargement :\n{ex.Message}", 
                    "OK");
            }
        }

        private void LoadSummaryData()
        {
            if (_membershipDetails == null) return;

            try
            {
                // Plan d'abonnement
                planTypeLabel.Text = _membershipDetails.MembershipPlan?.MembershipPlanType.ToString() ?? "Non défini";
                planDescriptionLabel.Text = _membershipDetails.MembershipPlan?.Description ?? "Description non disponible";

                // Club
                clubNameLabel.Text = _membershipDetails.HomeClub?.Name ?? "Non défini";

                // Dates
                startDateLabel.Text = _membershipDetails.StartDate.ToString("dd MMMM yyyy", CultureInfo.GetCultureInfo("fr-FR"));
                endDateLabel.Text = _membershipDetails.EndDate.ToString("dd MMMM yyyy", CultureInfo.GetCultureInfo("fr-FR"));

                // Renouvellement
                renewalLabel.Text = _membershipDetails.RenewWhenExpiry ? "Automatique" : "Manuel";

                // Prix - Utiliser le montant réel du PaymentDetail
                var basePrice = _membershipDetails.MembershipPlan?.BasePrice ?? 0;
                var registrationFees = _membershipDetails.MembershipPlan?.RegistrationFees ?? 0;
                var totalAmount = _membershipDetails.PaymentDetail?.Amount ?? 0; // Montant réel calculé

                basePriceLabel.Text = basePrice.ToString("C", CultureInfo.GetCultureInfo("fr-FR"));
                registrationFeesLabel.Text = registrationFees.ToString("C", CultureInfo.GetCultureInfo("fr-FR"));
                totalPriceLabel.Text = totalAmount.ToString("C", CultureInfo.GetCultureInfo("fr-FR"));
            }
            catch (Exception ex)
            {
                // En cas d'erreur, afficher des valeurs par défaut
                System.Diagnostics.Debug.WriteLine($"Erreur lors du chargement des données : {ex.Message}");
                
                planTypeLabel.Text = "Plan Standard";
                planDescriptionLabel.Text = "Abonnement fitness";
                clubNameLabel.Text = "Non défini";
                startDateLabel.Text = "Non défini";
                endDateLabel.Text = "Non défini";
                renewalLabel.Text = "Non défini";
                basePriceLabel.Text = "0,00 €";
                registrationFeesLabel.Text = "0,00 €";
                totalPriceLabel.Text = "0,00 €";
            }
        }

        private async void OnPayClicked(object sender, EventArgs e)
        {
            if (paymentMethodPicker.SelectedIndex == -1)
            {
                await DisplayAlert("⚠️ Attention", "Veuillez sélectionner une méthode de paiement pour continuer.", "OK");
                return;
            }

            try
            {
                // Animation et feedback du bouton
                payButton.IsEnabled = false;
                payButton.Text = "⏳ Traitement en cours...";
                await payButton.ScaleTo(0.95, 100);
                await payButton.ScaleTo(1.0, 100);

                // Mapper la sélection à l'enum PaymentMethod
                var selectedPaymentMethod = paymentMethodPicker.SelectedIndex switch
                {
                    0 => PaymentMethod.CreditCard,
                    1 => PaymentMethod.PayPal,
                    2 => PaymentMethod.BankTransfer,
                    3 => PaymentMethod.Cash,
                    4 => PaymentMethod.Other,
                    _ => PaymentMethod.CreditCard
                };

                var paymentDto = new PaymentDto
                {
                    PaymentMethod = selectedPaymentMethod
                };

                // Simulation d'un délai de traitement (optionnel)
                await Task.Delay(1500);

                var result = await _gymApiClient.PayMembershipAsync(UserId, MembershipId, paymentDto);

                if (result.Success)
                {
                    // Animation de succès
                    payButton.Text = "✅ Paiement réussi !";
                    payButton.BackgroundColor = Colors.Green;
                    await payButton.ScaleTo(1.1, 200);
                    await payButton.ScaleTo(1.0, 200);

                    var totalPaid = GetTotalPrice();
                    
                    await DisplayAlert("🎉 Félicitations !", 
                        $"Votre paiement de {totalPaid} a été effectué avec succès !\n\n" +
                        "Votre abonnement est maintenant actif. Bienvenue chez GymFit !", 
                        "Formidable !");

                    // Retour à la page principale
                    await Shell.Current.GoToAsync("//" + PageNames.MainPage);
                }
                else
                {
                    // Animation d'erreur
                    payButton.BackgroundColor = Colors.Red;
                    await payButton.ScaleTo(1.05, 100);
                    await payButton.ScaleTo(1.0, 100);

                    await DisplayAlert("❌ Échec du paiement", 
                        "Le paiement n'a pas pu être traité.\n\n" +
                        "Veuillez vérifier vos informations de paiement et réessayer.", 
                        "OK");

                    // Restaurer le bouton
                    payButton.Text = "💳 PROCÉDER AU PAIEMENT";
                    payButton.BackgroundColor = Color.FromArgb("#4CAF50"); // Success color
                    payButton.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("❌ Erreur", 
                    $"Une erreur inattendue s'est produite :\n{ex.Message}\n\n" +
                    "Veuillez réessayer ou contacter le support.", 
                    "OK");

                // Restaurer le bouton
                payButton.Text = "💳 PROCÉDER AU PAIEMENT";
                payButton.BackgroundColor = Color.FromArgb("#4CAF50"); // Success color
                payButton.IsEnabled = true;
            }
        }

        private string GetTotalPrice()
        {
            // Utiliser le montant réel du PaymentDetail au lieu de calculer manuellement
            if (_membershipDetails?.PaymentDetail != null)
            {
                return _membershipDetails.PaymentDetail.Amount.ToString("C", CultureInfo.GetCultureInfo("fr-FR"));
            }
            return "N/A";
        }
    }
}
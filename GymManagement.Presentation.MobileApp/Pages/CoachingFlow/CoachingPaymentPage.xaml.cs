using GymManagement.Application.Interfaces.Controllers.DTOs;
using GymManagement.Application.Interfaces.Services.Coachings;
using GymManagement.Presentation.MobileApp.ApiClients;
using GymManagement.Presentation.MobileApp.Services;
using GymManagement.Shared.Core.Enums;
using System.Globalization;

namespace GymManagement.Presentation.MobileApp.Pages.CoachingFlow
{
    [QueryProperty(nameof(UserId), "UserId")]
    [QueryProperty(nameof(CoachingId), "CoachingId")]
    public partial class CoachingPaymentPage : ContentPage
    {
        private readonly GymApiClient _gymApiClient;
        private CoachingDetailsDto? _coachingDetails;

        public int UserId { get; set; }
        public int CoachingId { get; set; }

        public List<string> PaymentMethods { get; } = new List<string>
        {
            "💳 Carte de crédit",
            "📱 PayPal",
            "🏦 Virement bancaire",
            "💰 Espèces",
            "💳 Autre méthode"
        };

        public CoachingPaymentPage()
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

            // Charger les détails du coaching
            await LoadCoachingDetails();
        }

        private async Task LoadCoachingDetails()
        {
            try
            {
                var coachingDetailsResult = await _gymApiClient.GetCoachingByIdAsync(UserId, CoachingId);
                if (coachingDetailsResult.Success)
                {
                    _coachingDetails = coachingDetailsResult.Results;
                    LoadSummaryData();
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
                    $"Une erreur s'est produite lors du chargement :\n{ex.Message}", 
                    "OK");
            }
        }

        private void LoadSummaryData()
        {
            if (_coachingDetails == null) return;

            try
            {
                // Nom du plan de coaching
                planNameLabel.Text = _coachingDetails.CoachingPlan?.Description ?? "Coaching Personnalisé";
                
                // Prix du coaching
                var price = _coachingDetails.CoachingPlan?.Price ?? 0;
                totalPriceLabel.Text = price.ToString("C", CultureInfo.GetCultureInfo("fr-FR"));

                // Date de début
                startDateLabel.Text = _coachingDetails.StartDate.ToString("dd MMMM yyyy", CultureInfo.GetCultureInfo("fr-FR"));

                // Jour et heure
                var dayName = GetDayName(_coachingDetails.WeekDay);
                var hour = _coachingDetails.Hour;
                
                scheduleLabel.Text = $"{dayName} à {hour:00}h00";
            }
            catch (Exception ex)
            {
                // En cas d'erreur, afficher des valeurs par défaut
                System.Diagnostics.Debug.WriteLine($"Erreur lors du chargement des données : {ex.Message}");
                
                planNameLabel.Text = "Coaching Personnalisé";
                totalPriceLabel.Text = "0,00 €";
                startDateLabel.Text = "Non défini";
                scheduleLabel.Text = "Non défini";
            }
        }

        private string GetDayName(WeekDays weekDay)
        {
            return weekDay switch
            {
                WeekDays.Monday => "Lundi",
                WeekDays.Tuesday => "Mardi",
                WeekDays.Wednesday => "Mercredi", 
                WeekDays.Thursday => "Jeudi",
                WeekDays.Friday => "Vendredi",
                WeekDays.Saturday => "Samedi",
                WeekDays.Sunday => "Dimanche",
                _ => "Non défini"
            };
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

                var result = await _gymApiClient.PayCoachingAsync(UserId, CoachingId, paymentDto);

                if (result.Success)
                {
                    // Animation de succès
                    payButton.Text = "✅ Paiement réussi !";
                    payButton.BackgroundColor = Colors.Green;
                    await payButton.ScaleTo(1.1, 200);
                    await payButton.ScaleTo(1.0, 200);

                    var totalPaid = _coachingDetails?.CoachingPlan?.Price.ToString("C", CultureInfo.GetCultureInfo("fr-FR")) ?? "N/A";
                    
                    await DisplayAlert("🎉 Félicitations !", 
                        $"Votre paiement de {totalPaid} a été effectué avec succès !\n\n" +
                        "Votre coaching est maintenant confirmé. Votre coach vous contactera bientôt pour planifier votre première séance.", 
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
    }
}
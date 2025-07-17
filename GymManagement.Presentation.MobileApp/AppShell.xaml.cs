using GymManagement.Presentation.MobileApp.Pages;
using GymManagement.Presentation.MobileApp.Pages.CoachingFlow;
using GymManagement.Presentation.MobileApp.Pages.SubscriptionFlow;

namespace GymManagement.Presentation.MobileApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(PageNames.RegisterPage, typeof(RegisterPage));
            Routing.RegisterRoute(PageNames.ProfilePage, typeof(ProfilePage));
            Routing.RegisterRoute(PageNames.MembershipsPage, typeof(MembershipsPage));
            Routing.RegisterRoute(PageNames.MembershipDetailsPage, typeof(MembershipDetailsPage));
            Routing.RegisterRoute(PageNames.CoachingPage, typeof(CoachingPage));
            Routing.RegisterRoute(PageNames.CoachingsPage, typeof(CoachingsPage));
            Routing.RegisterRoute(PageNames.CoachingDetailsPage, typeof(CoachingDetailsPage));
            Routing.RegisterRoute(PageNames.SubscriptionDetailsPage, typeof(SubscriptionDetailsPage));
            Routing.RegisterRoute(PageNames.SubscribePage, typeof(SubscribePage));
            Routing.RegisterRoute(PageNames.SubscriptionClubPage, typeof(SubscriptionClubPage));
            Routing.RegisterRoute(PageNames.PaymentPage, typeof(PaymentPage));
            Routing.RegisterRoute(PageNames.CoachingPaymentPage, typeof(CoachingPaymentPage));
            Routing.RegisterRoute(PageNames.CoachingFlowDetailsPage, typeof(CoachingFlowDetailsPage));
        }
    }
}

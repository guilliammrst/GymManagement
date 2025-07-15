using GymManagement.Application.Interfaces.Services.Clubs;
using GymManagement.Application.Interfaces.Services.MembershipPlans;
using GymManagement.Shared.Core.Enums;

namespace GymManagement.Presentation.MobileApp.Services
{
    public class SubscriptionFlowData
    {
        public int UserId { get; set; }
        public ClubDto HomeClub { get; set; }
        public MembershipPlanDto MembershipPlan { get; set; }
        public DateTime StartDate { get; set; }
        public bool RenewWhenExpiry { get; set; }
        public MembershipPeriod MembershipPeriod { get; set; }
    }
}

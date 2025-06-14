using GymManagement.Application.Interfaces.Repositories.Clubs;
using GymManagement.Application.Interfaces.Repositories.MembershipPlans;
using GymManagement.Application.Interfaces.Repositories.PaymentDetails;
using GymManagement.Application.Interfaces.Repositories.Users;
using GymManagement.Shared.Core.BaseObjects;

namespace GymManagement.Application.Interfaces.Repositories.Memberships
{
    public class MembershipDetailsDao : BaseDao
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool RenewWhenExpiry { get; set; }
        public bool IsExpired { get; set; }
        public UserDao Member { get; set; }
        public ClubDao HomeClub { get; set; }
        public MembershipPlanDao MembershipPlan { get; set; }
        public PaymentDetailDao PaymentDetail { get; set; }
    }
}

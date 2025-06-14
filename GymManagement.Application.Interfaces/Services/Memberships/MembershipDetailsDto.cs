using GymManagement.Application.Interfaces.Services.Clubs;
using GymManagement.Application.Interfaces.Services.MembershipPlans;
using GymManagement.Application.Interfaces.Services.PaymentDetails;
using GymManagement.Application.Interfaces.Services.Users;
using GymManagement.Shared.Core.BaseObjects;

namespace GymManagement.Application.Interfaces.Services.Memberships
{
    public class MembershipDetailsDto : BaseDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool RenewWhenExpiry { get; set; }
        public bool IsExpired { get; set; }
        public UserDto Member { get; set; }
        public ClubDto HomeClub { get; set; }
        public MembershipPlanDto MembershipPlan { get; set; }
        public PaymentDetailDto PaymentDetail { get; set; }
    }
}

using GymManagement.Shared.Core.Enums;

namespace GymManagement.Application.Interfaces.Services.Users
{
    public class UserSubscribeDto
    {
        public int UserId { get; set; }
        public int HomeClubId { get; set; }
        public int MembershipPlanId { get; set; }
        public DateTime? StartDate { get; set; }
        public bool? RenewWhenExpiry { get; set; }
        public MembershipPeriod? MembershipPeriod { get; set; }
    }
}

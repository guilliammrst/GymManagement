using GymManagement.Shared.Core.Enums;

namespace GymManagement.Presentation.Api.DTOs
{
    public class SubscribeUserDto
    {
        public int HomeClubId { get; set; }
        public int MembershipPlanId { get; set; }
        public DateTime? StartDate { get; set; }
        public bool? RenewWhenExpiry { get; set; }
        public MembershipPeriod? MembershipPeriod { get; set; }
    }
}

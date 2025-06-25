using GymManagement.Application.Interfaces.Services.Memberships;

namespace GymManagement.Application.Interfaces.Services.MembershipPlans
{
    public class MembershipPlanDetailsDto : MembershipPlanDto
    {
        public List<MembershipDto> Memberships { get; set; }
    }
}

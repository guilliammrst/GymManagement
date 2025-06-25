using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Interfaces.Repositories.MembershipPlans
{
    public interface IMembershipPlanCommandRepository
    {
        Task<ModelActionResult<MembershipPlanDao>> CreateMembershipPlanAsync(MembershipPlanCreateDao membershipPlanCreateDao);
        Task<ModelActionResult> UpdateMembershipPlanAsync(MembershipPlanUpdateDao membershipPlanUpdateDao);
    }
}

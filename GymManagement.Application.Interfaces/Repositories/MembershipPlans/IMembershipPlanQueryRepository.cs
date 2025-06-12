using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Interfaces.Repositories.MembershipPlans
{
    public interface IMembershipPlanQueryRepository
    {
        Task<ModelActionResult<MembershipPlanDetailsDao>> GetMembershipPlanByIdAsync(int id);
        Task<ModelActionResult<List<MembershipPlanDao>>> GetMembershipPlansAsync();
    }
}

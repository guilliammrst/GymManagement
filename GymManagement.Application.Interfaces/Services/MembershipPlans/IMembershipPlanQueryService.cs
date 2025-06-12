using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Interfaces.Services.MembershipPlans
{
    public interface IMembershipPlanQueryService
    {
        Task<ModelActionResult<MembershipPlanDetailsDto>> GetMembershipPlanByIdAsync(int id);
        Task<ModelActionResult<List<MembershipPlanDto>>> GetMembershipPlansAsync();
    }
}

using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Interfaces.Services.MembershipPlans
{
    public interface IMembershipPlanCommandService
    {
        Task<ModelActionResult<MembershipPlanDto>> CreateMembershipPlanAsync(MembershipPlanCreateDto membershipPlanCreateDto);
        Task<ModelActionResult> UpdateMembershipPlanAsync(MembershipPlanUpdateDto membershipPlanUpdateDto);
    }
}

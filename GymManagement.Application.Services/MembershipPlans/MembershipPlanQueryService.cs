using GymManagement.Application.Interfaces.Repositories.MembershipPlans;
using GymManagement.Application.Interfaces.Services.MembershipPlans;
using GymManagement.Application.Services.Converters;
using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Services.MembershipPlans
{
    public class MembershipPlanQueryService(IMembershipPlanQueryRepository _membershipPlanQueryRepository) : IMembershipPlanQueryService
    {
        public async Task<ModelActionResult<MembershipPlanDetailsDto>> GetMembershipPlanByIdAsync(int clubId)
        {
            var membershipPlanResult = await _membershipPlanQueryRepository.GetMembershipPlanByIdAsync(clubId);
            if (!membershipPlanResult.Success)
                return ModelActionResult<MembershipPlanDetailsDto>.Fail(membershipPlanResult);

            var membershipPlan = membershipPlanResult.Results;

            return ModelActionResult<MembershipPlanDetailsDto>.Ok(membershipPlan.ToDetailsDto());
        }

        public async Task<ModelActionResult<List<MembershipPlanDto>>> GetMembershipPlansAsync()
        {
            var membershipPlansResult = await _membershipPlanQueryRepository.GetMembershipPlansAsync();
            if (!membershipPlansResult.Success)
                return ModelActionResult<List<MembershipPlanDto>>.Fail(membershipPlansResult);

            var membershipPlans = membershipPlansResult.Results;

            return ModelActionResult<List<MembershipPlanDto>>.Ok(membershipPlans.ToDto());
        }
    }
}

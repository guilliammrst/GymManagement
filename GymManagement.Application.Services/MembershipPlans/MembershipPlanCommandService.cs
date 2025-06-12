using GymManagement.Application.Interfaces.Repositories.MembershipPlans;
using GymManagement.Application.Interfaces.Services.MembershipPlans;
using GymManagement.Application.Services.Converters;
using GymManagement.Domain;
using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Services.MembershipPlans
{
    public class MembershipPlanCommandService(IMembershipPlanCommandRepository _membershipPlanCommandRepository) : IMembershipPlanCommandService
    {
        public async Task<ModelActionResult<MembershipPlanDto>> CreateMembershipPlanAsync(MembershipPlanCreateDto membershipPlanCreateDto)
        {
            var membershipPlanCreateResult = MembershipPlan.Create(membershipPlanCreateDto.Description, membershipPlanCreateDto.MembershipPlanType, membershipPlanCreateDto.BasePrice, membershipPlanCreateDto.YearlyDiscount, membershipPlanCreateDto.RegistrationFees);
            if (!membershipPlanCreateResult.Success)
                return ModelActionResult<MembershipPlanDto>.Fail(membershipPlanCreateResult);

            var membershipPlan = membershipPlanCreateResult.Results;

            var newMembershipPlanResult = await _membershipPlanCommandRepository.CreateMembershipPlanAsync(membershipPlan.ToCreateDao());
            if (!newMembershipPlanResult.Success)
                return ModelActionResult<MembershipPlanDto>.Fail(newMembershipPlanResult);

            var newMembershipPlan = newMembershipPlanResult.Results;

            return ModelActionResult<MembershipPlanDto>.Ok(newMembershipPlan.ToDto());
        }
    }
}

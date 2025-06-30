using GymManagement.Application.Interfaces.Repositories.MembershipPlans;
using GymManagement.Application.Interfaces.Services.MembershipPlans;
using GymManagement.Application.Services.Converters;
using GymManagement.Domain;
using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Services.MembershipPlans
{
    public class MembershipPlanCommandService(IMembershipPlanCommandRepository _membershipPlanCommandRepository, IMembershipPlanQueryRepository _membershipPlanQueryRepository) : IMembershipPlanCommandService
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

        public async Task<ModelActionResult> UpdateMembershipPlanAsync(MembershipPlanUpdateDto membershipPlanUpdateDto)
        {
            var membershipPlanResult = await _membershipPlanQueryRepository.GetMembershipPlanByIdAsync(membershipPlanUpdateDto.Id);
            if (!membershipPlanResult.Success)
                return ModelActionResult.Fail(membershipPlanResult);

            var membershipPlanDao = membershipPlanResult.Results;

            var membershipPlanBuildResult = MembershipPlan.Build(membershipPlanDao.Id, membershipPlanDao.CreatedAt, membershipPlanDao.UpdatedAt, membershipPlanDao.IsValid, membershipPlanDao.Description, membershipPlanDao.MembershipPlanType, membershipPlanDao.BasePrice, membershipPlanDao.YearlyDiscount, membershipPlanDao.RegistrationFees);
            if (!membershipPlanBuildResult.Success)
                return ModelActionResult.Fail(membershipPlanBuildResult);

            var membershipPlan = membershipPlanBuildResult.Results;

            var updateMembershipPlanResult = membershipPlan.Update(membershipPlanUpdateDto.IsValid, membershipPlanUpdateDto.Description, membershipPlanUpdateDto.MembershipPlanType, membershipPlanUpdateDto.BasePrice, membershipPlanUpdateDto.YearlyDiscount, membershipPlanUpdateDto.RegistrationFees);
            if (!updateMembershipPlanResult.Success)
                return ModelActionResult.Fail(updateMembershipPlanResult);

            var updatedMembershipPlanResult = await _membershipPlanCommandRepository.UpdateMembershipPlanAsync(membershipPlan.ToUpdateDao());
            if (!updatedMembershipPlanResult.Success)
                return ModelActionResult.Fail(updatedMembershipPlanResult);

            return ModelActionResult.Ok;
        }
    }
}

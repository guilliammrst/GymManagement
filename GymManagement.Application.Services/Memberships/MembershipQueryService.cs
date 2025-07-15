using GymManagement.Application.Interfaces.Repositories.Memberships;
using GymManagement.Application.Interfaces.Services.Memberships;
using GymManagement.Application.Services.Converters;
using GymManagement.Shared.Core.Enums;
using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Services.Memberships
{
    public class MembershipQueryService(IMembershipQueryRepository _membershipQueryRepository) : IMembershipQueryService
    {
        public async Task<ModelActionResult<MembershipDetailsDto>> GetMembershipByIdAsync(int userId, int membershipId)
        {
            var membershipResult = await _membershipQueryRepository.GetMembershipByIdAsync(membershipId);
            if (!membershipResult.Success)
                return ModelActionResult<MembershipDetailsDto>.Fail(membershipResult);

            var membership = membershipResult.Results;

            if (membership.Member.Id != userId)
                return ModelActionResult<MembershipDetailsDto>.Fail(GymFaultType.UserNotAuthorized);

            return ModelActionResult<MembershipDetailsDto>.Ok(membership.ToDetailsDto());
        }
    }
}

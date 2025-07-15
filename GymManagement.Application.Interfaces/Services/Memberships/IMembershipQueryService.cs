using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Interfaces.Services.Memberships
{
    public interface IMembershipQueryService
    {
        Task<ModelActionResult<MembershipDetailsDto>> GetMembershipByIdAsync(int userId, int membershipId);
    }
}

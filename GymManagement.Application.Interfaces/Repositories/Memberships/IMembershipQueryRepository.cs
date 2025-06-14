using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Interfaces.Repositories.Memberships
{
    public interface IMembershipQueryRepository
    {
        Task<ModelActionResult<MembershipDetailsDao>> GetMembershipByIdAsync(int id);
    }
}

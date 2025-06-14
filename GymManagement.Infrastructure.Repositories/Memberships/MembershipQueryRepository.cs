using GymManagement.Application.Interfaces.Repositories.Memberships;
using GymManagement.Infrastructure.Repositories.Converters;
using GymManagement.Infrastructure.Repositories.DbContexts;
using GymManagement.Shared.Core.Enums;
using GymManagement.Shared.Core.Results;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Repositories.Memberships
{
    public class MembershipQueryRepository(GymDbContext _context) : IMembershipQueryRepository
    {
        public async Task<ModelActionResult<MembershipDetailsDao>> GetMembershipByIdAsync(int id)
        {
            var membershipModel = await _context.Memberships
                .Include(m => m.Member)
                .Include(m => m.MembershipPlan)
                .Include(m => m.HomeClub)
                .Include(m => m.PaymentDetail)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (membershipModel == null)
                return ModelActionResult<MembershipDetailsDao>.Fail(GymFaultType.MembershipNotFound, "Get membership failed: membership not found.");

            return ModelActionResult<MembershipDetailsDao>.Ok(membershipModel.ToDetailsDao());
        }
    }
}

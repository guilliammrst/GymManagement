using GymManagement.Application.Interfaces.Repositories.MembershipPlans;
using GymManagement.Infrastructure.Repositories.Converters;
using GymManagement.Infrastructure.Repositories.DbContexts;
using GymManagement.Shared.Core.Enums;
using GymManagement.Shared.Core.Results;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Repositories.MembershipPlans
{
    public class MembershipPlanQueryRepository(GymDbContext _context) : IMembershipPlanQueryRepository
    {
        public async Task<ModelActionResult<MembershipPlanDetailsDao>> GetMembershipPlanByIdAsync(int id)
        {
            var membershipPlanModel = await _context.MembershipPlans
                .Include(mp => mp.Memberships)
                .FirstOrDefaultAsync(mp => mp.Id == id);

            if (membershipPlanModel == null)
                return ModelActionResult<MembershipPlanDetailsDao>.Fail(GymFaultType.MembershipPlanNotFound, "Get membership plan failed: membership plan not found.");

            var membershipPlan = membershipPlanModel.ToDetailsDao();
            return ModelActionResult<MembershipPlanDetailsDao>.Ok(membershipPlan);
        }

        public async Task<ModelActionResult<List<MembershipPlanDao>>> GetMembershipPlansAsync()
        {
            var membershipPlans = await _context.MembershipPlans.ToListAsync();
            
            return ModelActionResult<List<MembershipPlanDao>>.Ok(membershipPlans.ToDao());
        }
    }
}

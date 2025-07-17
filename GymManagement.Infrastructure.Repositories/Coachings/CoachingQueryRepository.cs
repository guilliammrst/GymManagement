using GymManagement.Application.Interfaces.Repositories.CoachingPlans;
using GymManagement.Application.Interfaces.Repositories.Coachings;
using GymManagement.Infrastructure.Repositories.Converters;
using GymManagement.Infrastructure.Repositories.DbContexts;
using GymManagement.Shared.Core.Enums;
using GymManagement.Shared.Core.Results;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Repositories.Coachings
{
    public class CoachingQueryRepository(GymDbContext _context) : ICoachingQueryRepository
    {
        public async Task<ModelActionResult<CoachingDetailsDao>> GetCoachingByIdAsync(int id)
        {
            var coachingModel = await _context.Coachings
               .Include(mp => mp.CoachingPlan)
               .Include(mp => mp.PaymentDetail)
               .Include(mp => mp.Member)
               .FirstOrDefaultAsync(mp => mp.Id == id);

            if (coachingModel == null)
                return ModelActionResult<CoachingDetailsDao>.Fail(GymFaultType.CoachingPlanNotFound, "Get coaching failed: coaching not found.");

            var coachingPlan = coachingModel.ToDetailsDao();
            return ModelActionResult<CoachingDetailsDao>.Ok(coachingPlan);
        }

        public async Task<ModelActionResult<List<CoachingDao>>> GetCoachingsAsync(CoachingRepositoryFilter filter)
        {
            var coachings = await _context.Coachings
                .Include(mp => mp.CoachingPlan)
                .Where(c => (!filter.MemberId.HasValue || c.MemberId == filter.MemberId) && (!filter.CoachId.HasValue || c.CoachingPlan!.CoachId == filter.CoachId.Value))
                .ToListAsync();

            return ModelActionResult<List<CoachingDao>>.Ok(coachings.ToDao());
        }
    }
}

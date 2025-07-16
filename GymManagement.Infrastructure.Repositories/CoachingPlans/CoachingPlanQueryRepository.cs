using GymManagement.Application.Interfaces.Repositories.CoachingPlans;
using GymManagement.Infrastructure.Repositories.Converters;
using GymManagement.Infrastructure.Repositories.DbContexts;
using GymManagement.Shared.Core.Enums;
using GymManagement.Shared.Core.Results;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Repositories.CoachingPlans
{
    public class CoachingPlanQueryRepository(GymDbContext _context) : ICoachingPlanQueryRepository
    {
        public async Task<ModelActionResult<CoachingPlanDetailsDao>> GetCoachingPlanByIdAsync(int id)
        {
            var coachingPlanModel = await _context.CoachingPlans
                .Include(mp => mp.Coachings)
                .Include(mp => mp.Coach)
                .Include(mp => mp.Club)
                .FirstOrDefaultAsync(mp => mp.Id == id);

            if (coachingPlanModel == null)
                return ModelActionResult<CoachingPlanDetailsDao>.Fail(GymFaultType.CoachingPlanNotFound, "Get coaching plan failed: coaching plan not found.");

            var coachingPlan = coachingPlanModel.ToDetailsDao();
            return ModelActionResult<CoachingPlanDetailsDao>.Ok(coachingPlan);
        }

        public async Task<ModelActionResult<List<CoachingPlanDao>>> GetCoachingPlansAsync(CoachingPlanRepositoryFilter filter)
        {
            var coachingPlans = await _context.CoachingPlans
                .Include(mp => mp.Coach)
                .ToListAsync();
            
            if (filter.ClubId.HasValue)
                coachingPlans = [.. coachingPlans.Where(mp => mp.ClubId == filter.ClubId.Value)];
            
            if (!string.IsNullOrWhiteSpace(filter.CoachEmail))
                coachingPlans = [.. coachingPlans.Where(mp => mp.Coach!.Email == filter.CoachEmail)];

            return ModelActionResult<List<CoachingPlanDao>>.Ok(coachingPlans.ToDao());
        }
    }
}

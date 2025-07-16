using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Interfaces.Repositories.CoachingPlans
{
    public interface ICoachingPlanQueryRepository
    {
        Task<ModelActionResult<CoachingPlanDetailsDao>> GetCoachingPlanByIdAsync(int? id);
        Task<ModelActionResult<List<CoachingPlanDao>>> GetCoachingPlansAsync(CoachingPlanRepositoryFilter filter);
    }
}

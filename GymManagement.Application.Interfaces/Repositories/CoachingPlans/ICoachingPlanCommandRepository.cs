using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Interfaces.Repositories.CoachingPlans
{
    public interface ICoachingPlanCommandRepository
    {
        Task<ModelActionResult<CoachingPlanDetailsDao>> CreateCoachingPlanAsync(CoachingPlanCreateDao coachingPlanCreateDao);
    }
}

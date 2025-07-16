using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Interfaces.Services.CoachingPlans
{
    public interface ICoachingPlanQueryService
    {
        Task<ModelActionResult<CoachingPlanDetailsDto>> GetCoachingPlanByIdAsync(int id);
        Task<ModelActionResult<List<CoachingPlanDto>>> GetCoachingPlansAsync(CoachingPlanServiceFilter filter);
    }
}

using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Interfaces.Services.CoachingPlans
{
    public interface ICoachingPlanCommandService
    {
        Task<ModelActionResult<CoachingPlanDetailsDto>> CreateCoachingPlanAsync(CoachingPlanCreateDto coachingPlanCreateDto);
    }
}

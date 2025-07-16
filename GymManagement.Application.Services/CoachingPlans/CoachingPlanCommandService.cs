using GymManagement.Application.Interfaces.Repositories.CoachingPlans;
using GymManagement.Application.Interfaces.Services.CoachingPlans;
using GymManagement.Application.Services.Converters;
using GymManagement.Domain;
using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Services.CoachingPlans
{
    public class CoachingPlanCommandService(ICoachingPlanCommandRepository _coachingPlanCommandRepository) : ICoachingPlanCommandService
    {
        public async Task<ModelActionResult<CoachingPlanDetailsDto>> CreateCoachingPlanAsync(CoachingPlanCreateDto coachingPlanCreateDto)
        {
            var coachingPlanResult = CoachingPlan.Create(coachingPlanCreateDto.Description, coachingPlanCreateDto.Price, coachingPlanCreateDto.CoachId, coachingPlanCreateDto.ClubId);
            if (!coachingPlanResult.Success)
                return ModelActionResult<CoachingPlanDetailsDto>.Fail(coachingPlanResult);

            var coachingPlan = coachingPlanResult.Results;

            var createCoachingPlanResult = await _coachingPlanCommandRepository.CreateCoachingPlanAsync(coachingPlan.ToCreateDao());
            if (!createCoachingPlanResult.Success)
                return ModelActionResult<CoachingPlanDetailsDto>.Fail(createCoachingPlanResult);

            var createdCoachingPlan = createCoachingPlanResult.Results;

            return ModelActionResult<CoachingPlanDetailsDto>.Ok(createdCoachingPlan.ToDetailsDto());
        }
    }
}

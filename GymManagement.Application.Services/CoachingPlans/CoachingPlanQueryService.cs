using GymManagement.Application.Interfaces.Repositories.CoachingPlans;
using GymManagement.Application.Interfaces.Services.CoachingPlans;
using GymManagement.Application.Services.Converters;
using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Services.CoachingPlans
{
    public class CoachingPlanQueryService(ICoachingPlanQueryRepository _coachingPlanQueryRepository) : ICoachingPlanQueryService
    {
        public async Task<ModelActionResult<CoachingPlanDetailsDto>> GetCoachingPlanByIdAsync(int id)
        {
            var coachingPlanResult = await _coachingPlanQueryRepository.GetCoachingPlanByIdAsync(id);
            if (!coachingPlanResult.Success)
                return ModelActionResult<CoachingPlanDetailsDto>.Fail(coachingPlanResult);

            var coachingPlanDetailsDto = coachingPlanResult.Results.ToDetailsDto();

            return ModelActionResult<CoachingPlanDetailsDto>.Ok(coachingPlanDetailsDto);
        }

        public async Task<ModelActionResult<List<CoachingPlanDto>>> GetCoachingPlansAsync(CoachingPlanServiceFilter filter)
        {
            var coachingPlanResult = await _coachingPlanQueryRepository.GetCoachingPlansAsync(new CoachingPlanRepositoryFilter
            {
                ClubId = filter.ClubId,
                CoachEmail = filter.CoachEmail
            });
            if (!coachingPlanResult.Success)
                return ModelActionResult<List<CoachingPlanDto>>.Fail(coachingPlanResult);

            var coachingPlansDto = coachingPlanResult.Results.ToDto();
            
            return ModelActionResult<List<CoachingPlanDto>>.Ok(coachingPlansDto);
        }
    }
}

using GymManagement.Application.Interfaces.Repositories.Coachings;
using GymManagement.Application.Interfaces.Services.Coachings;
using GymManagement.Application.Services.Converters;
using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Services.Coachings
{
    public class CoachingQueryService(ICoachingQueryRepository _coachingQueryRepository) : ICoachingQueryService
    {
        public async Task<ModelActionResult<CoachingDetailsDto>> GetCoachingByIdAsync(int id)
        { 
            var coachingResult = await _coachingQueryRepository.GetCoachingByIdAsync(id);
            if (!coachingResult.Success)
                return ModelActionResult<CoachingDetailsDto>.Fail(coachingResult);

            var coaching = coachingResult.Results;

            return ModelActionResult<CoachingDetailsDto>.Ok(coaching.ToDetailsDto());
        }

        public async Task<ModelActionResult<List<CoachingDto>>> GetCoachingsAsync(CoachingServiceFilter filter)
        {
            var coachingResult = await _coachingQueryRepository.GetCoachingsAsync(new CoachingRepositoryFilter
            {
                CoachId = filter.CoachId,
                MemberId = filter.MemberId
            });
            if (!coachingResult.Success)
                return ModelActionResult<List<CoachingDto>>.Fail(coachingResult);

            var coachings = coachingResult.Results;
            
            return ModelActionResult<List<CoachingDto>>.Ok(coachings.ToDto());
        }
    }
}

using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Interfaces.Services.Coachings
{
    public interface ICoachingQueryService
    {
        Task<ModelActionResult<CoachingDetailsDto>> GetCoachingByIdAsync(int id);
        Task<ModelActionResult<List<CoachingDto>>> GetCoachingsAsync(CoachingServiceFilter filter);
    }
}

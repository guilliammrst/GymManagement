using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Interfaces.Repositories.Coachings
{
    public interface ICoachingQueryRepository
    {
        Task<ModelActionResult<CoachingDetailsDao>> GetCoachingByIdAsync(int id);
        Task<ModelActionResult<List<CoachingDao>>> GetCoachingsAsync(CoachingRepositoryFilter filter);
    }
}

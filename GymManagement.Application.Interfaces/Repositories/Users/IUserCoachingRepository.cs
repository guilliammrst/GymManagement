using GymManagement.Application.Interfaces.Repositories.Coachings;
using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Interfaces.Repositories.Users
{
    public interface IUserCoachingRepository
    {
        Task<ModelActionResult<CoachingDetailsDao>> AddUserCoachingAsync(UserCoachingDao userCoachingDao);
        Task<ModelActionResult<CoachingDetailsDao>> PayUserCoachingAsync(UserPaymentDao userPaymentDao);
    }
}

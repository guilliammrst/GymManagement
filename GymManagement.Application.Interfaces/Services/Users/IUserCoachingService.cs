using GymManagement.Application.Interfaces.Services.Coachings;
using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Interfaces.Services.Users
{
    public interface IUserCoachingService
    {
        Task<ModelActionResult<UserDetailsDto>> AddUserCoachingAsync(UserCoachingDto userCoachingDto);
        Task<ModelActionResult<CoachingDetailsDto>> PayUserCoachingAsync(UserPaymentDto userPaymentDto);
    }
}

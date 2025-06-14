using GymManagement.Application.Interfaces.Services.Memberships;
using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Interfaces.Services.Users
{
    public interface IUserSubscriptionService
    {
        Task<ModelActionResult<UserDetailsDto>> AddUserSubscriptionAsync(UserSubscribeDto userSubscribeDto);
        Task<ModelActionResult<MembershipDetailsDto>> PayUserSubscriptionAsync(UserPaymentDto userPaymentDto);
    }
}

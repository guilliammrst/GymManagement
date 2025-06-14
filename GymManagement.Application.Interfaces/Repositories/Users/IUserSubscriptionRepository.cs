using GymManagement.Application.Interfaces.Repositories.Memberships;
using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Interfaces.Repositories.Users
{
    public interface IUserSubscriptionRepository
    {
        Task<ModelActionResult<UserDetailsDao>> SubscribeUserAsync(UserSubscribeDao userSubscribeDao);
        Task<ModelActionResult<MembershipDetailsDao>> PayUserSubscriptionAsync(UserPaymentDao userPaymentDao);
    }
}

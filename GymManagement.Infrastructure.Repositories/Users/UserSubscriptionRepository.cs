using GymManagement.Application.Interfaces.Repositories.Memberships;
using GymManagement.Application.Interfaces.Repositories.Users;
using GymManagement.Infrastructure.Repositories.Converters;
using GymManagement.Infrastructure.Repositories.DbContexts;
using GymManagement.Shared.Core.Enums;
using GymManagement.Shared.Core.Results;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Repositories.Users
{
    public class UserSubscriptionRepository(GymDbContext _context) : IUserSubscriptionRepository
    {
        public async Task<ModelActionResult<UserDetailsDao>> SubscribeUserAsync(UserSubscribeDao userSubscribeDao)
        {
            try
            {
                var userModel = await _context.Users.Include(u => u.Memberships)
                    .FirstOrDefaultAsync(u => u.Id == userSubscribeDao.UserId);
                if (userModel == null)
                    return ModelActionResult<UserDetailsDao>.Fail(GymFaultType.UserNotFound, "User subscription failed: user not found.");

                var homeClubModel = await _context.Clubs.FindAsync(userSubscribeDao.HomeClubId);
                if (homeClubModel == null)
                    return ModelActionResult<UserDetailsDao>.Fail(GymFaultType.ClubNotFound, "User subscription failed: home club not found.");

                var membershipPlanModel = await _context.MembershipPlans.FindAsync(userSubscribeDao.MembershipPlanId);
                if (membershipPlanModel == null)
                    return ModelActionResult<UserDetailsDao>.Fail(GymFaultType.MembershipPlanNotFound, "User subscription failed: membership plan not found.");

                var membershipModel = userSubscribeDao.ToModel(membershipPlanModel, homeClubModel);

                if (userModel.Role == Role.None)
                    userModel.Role = Role.Member;

                userModel.Memberships.Add(membershipModel);
                _context.Users.Update(userModel);

                var result = await _context.SaveChangesAsync();
                if (result == 0)
                    return ModelActionResult<UserDetailsDao>.Fail(GymFaultType.UserSubscriptionFailed, "User subscription failed: no rows affected.");

                return ModelActionResult<UserDetailsDao>.Ok(userModel.ToDetailsDao());
            }
            catch (DbUpdateException ex)
            {
                return ModelActionResult<UserDetailsDao>.Fail(GymFaultType.DatabaseUnavailable, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
            catch (Exception ex)
            {
                return ModelActionResult<UserDetailsDao>.Fail(GymFaultType.UserSubscriptionFailed, ex.Message);
            }
        }

        public async Task<ModelActionResult<MembershipDetailsDao>> PayUserSubscriptionAsync(UserPaymentDao userPaymentDao)
        {
            try
            {
                var membershipModel = await _context.Memberships.Include(m => m.PaymentDetail)
                    .Include(m => m.MembershipPlan)
                    .Include(m => m.HomeClub)
                    .Include(m => m.Member)
                    .FirstOrDefaultAsync(m => m.Id == userPaymentDao.MembershipId);
                if (membershipModel == null)
                    return ModelActionResult<MembershipDetailsDao>.Fail(GymFaultType.MembershipNotFound, "Membership payment failed: membership not found.");

                if (membershipModel.PaymentDetail is null)
                    return ModelActionResult<MembershipDetailsDao>.Fail(GymFaultType.MembershipPaymentDetailNotFound, "Membership payment failed: membership payment detail not found.");

                membershipModel.IsActive = userPaymentDao.IsActive;
                membershipModel.PaymentDetail.PaymentMethod = userPaymentDao.PaymentMethod;
                membershipModel.PaymentDetail.PaymentStatus = userPaymentDao.PaymentStatus;
                membershipModel.PaymentDetail.PaymentDate = userPaymentDao.PaymentDate;

                _context.Memberships.Update(membershipModel);
                var result = await _context.SaveChangesAsync();
                if (result == 0)
                    return ModelActionResult<MembershipDetailsDao>.Fail(GymFaultType.MembershipPaymentFailed, "Membership payment failed: no rows affected.");

                return ModelActionResult<MembershipDetailsDao>.Ok(membershipModel.ToDetailsDao());
            }
            catch (DbUpdateException ex)
            {
                return ModelActionResult<MembershipDetailsDao>.Fail(GymFaultType.DatabaseUnavailable, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
            catch (Exception ex)
            {
                return ModelActionResult<MembershipDetailsDao>.Fail(GymFaultType.MembershipPaymentFailed, ex.Message);
            }
        }
    }
}

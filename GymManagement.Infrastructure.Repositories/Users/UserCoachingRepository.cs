using GymManagement.Application.Interfaces.Repositories.Coachings;
using GymManagement.Application.Interfaces.Repositories.Memberships;
using GymManagement.Application.Interfaces.Repositories.Users;
using GymManagement.Infrastructure.Repositories.Converters;
using GymManagement.Infrastructure.Repositories.DbContexts;
using GymManagement.Shared.Core.Enums;
using GymManagement.Shared.Core.Results;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Repositories.Users
{
    public class UserCoachingRepository(GymDbContext _context) : IUserCoachingRepository
    {
        public async Task<ModelActionResult<UserDetailsDao>> AddUserCoachingAsync(UserCoachingDao userCoachingDao)
        {
            try
            {
                var coachingModel = userCoachingDao.ToModel();

                var userModel = await _context.Users.Include(u => u.Memberships)
                    .FirstOrDefaultAsync(u => u.Id == userCoachingDao.MemberId);
                if (userModel == null)
                    return ModelActionResult<UserDetailsDao>.Fail(GymFaultType.UserNotFound, "User add coaching failed: user not found.");

                var coachingPlanModel = await _context.CoachingPlans
                    .Include(cp => cp.Coachings)
                    .FirstOrDefaultAsync(cp => cp.Id == userCoachingDao.CoachingPlanId);
                if (coachingPlanModel == null)
                    return ModelActionResult<UserDetailsDao>.Fail(GymFaultType.CoachingPlanNotFound, "User add coaching failed: coaching plan not found.");

                coachingModel.CoachingPlan = coachingPlanModel;
                coachingModel.Member = userModel;

                _context.Coachings.Add(coachingModel);

                var result = await _context.SaveChangesAsync();
                if (result == 0)
                    return ModelActionResult<UserDetailsDao>.Fail(GymFaultType.UserAddCoachingFailed, "User add coaching failed: no rows affected.");

                return ModelActionResult<UserDetailsDao>.Ok(userModel.ToDetailsDao());
            }
            catch (DbUpdateException ex)
            {
                return ModelActionResult<UserDetailsDao>.Fail(GymFaultType.DatabaseUnavailable, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
            catch (Exception ex)
            {
                return ModelActionResult<UserDetailsDao>.Fail(GymFaultType.UserAddCoachingFailed, ex.Message);
            }
        }

        public async Task<ModelActionResult<CoachingDetailsDao>> PayUserCoachingAsync(UserPaymentDao userPaymentDao)
        {
            try
            {
                var coachingModel = await _context.Coachings
                    .Include(m => m.PaymentDetail)
                    .Include(m => m.CoachingPlan)
                    .Include(m => m.Member)
                    .FirstOrDefaultAsync(m => m.Id == userPaymentDao.EntityId);
                if (coachingModel == null)
                    return ModelActionResult<CoachingDetailsDao>.Fail(GymFaultType.CoachingNotFound, "Coaching payment failed: Coaching not found.");

                if (coachingModel.PaymentDetail is null)
                    return ModelActionResult<CoachingDetailsDao>.Fail(GymFaultType.CoachingPaymentDetailNotFound, "Coaching payment failed: Coaching payment detail not found.");

                coachingModel.IsActive = userPaymentDao.IsActive;
                coachingModel.PaymentDetail.PaymentMethod = userPaymentDao.PaymentMethod;
                coachingModel.PaymentDetail.PaymentStatus = userPaymentDao.PaymentStatus;
                coachingModel.PaymentDetail.PaymentDate = userPaymentDao.PaymentDate;

                _context.Coachings.Update(coachingModel);
                var result = await _context.SaveChangesAsync();
                if (result == 0)
                    return ModelActionResult<CoachingDetailsDao>.Fail(GymFaultType.MembershipPaymentFailed, "Coaching payment failed: no rows affected.");

                return ModelActionResult<CoachingDetailsDao>.Ok(coachingModel.ToDetailsDao());
            }
            catch (DbUpdateException ex)
            {
                return ModelActionResult<CoachingDetailsDao>.Fail(GymFaultType.DatabaseUnavailable, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
            catch (Exception ex)
            {
                return ModelActionResult<CoachingDetailsDao>.Fail(GymFaultType.MembershipPaymentFailed, ex.Message);
            }
        }
    }
}

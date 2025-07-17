using GymManagement.Application.Interfaces.Repositories.CoachingPlans;
using GymManagement.Application.Interfaces.Repositories.Coachings;
using GymManagement.Application.Interfaces.Repositories.Users;
using GymManagement.Application.Interfaces.Services.Coachings;
using GymManagement.Application.Interfaces.Services.Memberships;
using GymManagement.Application.Interfaces.Services.Users;
using GymManagement.Application.Services.Converters;
using GymManagement.Domain;
using GymManagement.Shared.Core.Enums;
using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Services.Users
{
    public class UserCoachingService(IUserCoachingRepository _userCoachingRepository,
        IUserQueryRepository _userQueryRepository,
        ICoachingPlanQueryRepository _coachingPlanQueryRepository,
        ICoachingQueryRepository _coachingQueryRepository) : IUserCoachingService
    {
        public async Task<ModelActionResult<CoachingDetailsDto>> AddUserCoachingAsync(UserCoachingDto userCoachingDto)
        {
            var userResult = await _userQueryRepository.GetUserByIdAsync(userCoachingDto.MemberId);
            if (!userResult.Success)
                return ModelActionResult<CoachingDetailsDto>.Fail(userResult);

            var userDao = userResult.Results;
            var user = User.Build(userDao.Id, userDao.CreatedAt, userDao.UpdatedAt, userDao.Name, userDao.Surname, userDao.Birthdate,
                userDao.Password, userDao.Role, userDao.Email, userDao.PhoneNumber, userDao.Gender, userDao.Address.Country, userDao.Address.City,
                userDao.Address.Street, userDao.Address.PostalCode, userDao.Address.Number, userDao.Memberships.Build()).Results;

            var coachingPlanResult = await _coachingPlanQueryRepository.GetCoachingPlanByIdAsync(userCoachingDto.CoachingPlanId);
            if (!coachingPlanResult.Success)
                return ModelActionResult<CoachingDetailsDto>.Fail(coachingPlanResult);

            var coachingPlanDao = coachingPlanResult.Results;
            var coachingPlan = CoachingPlan.Build(coachingPlanDao.Id, coachingPlanDao.CreatedAt, coachingPlanDao.UpdatedAt,
                coachingPlanDao.IsValid, coachingPlanDao.Description, coachingPlanDao.Price).Results;

            var coachingResult = Coaching.Create(userCoachingDto.StartDate, userCoachingDto.RenewWhenExpiry, userCoachingDto.WeekDay, userCoachingDto.Hour, coachingPlan, user);
            if (!coachingResult.Success)
                return ModelActionResult<CoachingDetailsDto>.Fail(coachingResult);

            var coaching = coachingResult.Results;

            var userCoachingResult = await _userCoachingRepository.AddUserCoachingAsync(coaching.ToUserCoachingDao());
            if (!userCoachingResult.Success)
                return ModelActionResult<CoachingDetailsDto>.Fail(userCoachingResult);

            var updatedUserDao = userCoachingResult.Results;
            return ModelActionResult<CoachingDetailsDto>.Ok(updatedUserDao.ToDetailsDto());
        }

        public async Task<ModelActionResult<CoachingDetailsDto>> PayUserCoachingAsync(UserPaymentDto userPaymentDto)
        {
            var coachingResult = await _coachingQueryRepository.GetCoachingByIdAsync(userPaymentDto.EntityId);
            if (!coachingResult.Success)
                return ModelActionResult<CoachingDetailsDto>.Fail(coachingResult);

            var coachingDao = coachingResult.Results;

            if (coachingDao.Member.Id != userPaymentDto.UserId)
                return ModelActionResult<CoachingDetailsDto>.Fail(GymFaultType.BadParameter, "Pay coaching failed: coaching's userId is not the same that the userId provided.");

            var paymentDetailDao = coachingDao.PaymentDetail;

            var paymentDetailResult = PaymentDetail.Build(paymentDetailDao.Id, paymentDetailDao.CreatedAt, paymentDetailDao.UpdatedAt, paymentDetailDao.Amount,
                paymentDetailDao.PaymentMethod, paymentDetailDao.PaymentStatus, paymentDetailDao.PaymentDate);
            if (!paymentDetailResult.Success)
                return ModelActionResult<CoachingDetailsDto>.Fail(paymentDetailResult);

            var coaching = Coaching.Build(coachingDao.Id, coachingDao.CreatedAt, coachingDao.UpdatedAt, coachingDao.StartDate, coachingDao.EndDate, 
                coachingDao.IsActive, coachingDao.RenewWhenExpiry, coachingDao.WeekDay, coachingDao.Hour, paymentDetailResult.Results).Results;

            var payCoachingResult = coaching.Pay(userPaymentDto.PaymentMethod);
            if (!payCoachingResult.Success)
                return ModelActionResult<CoachingDetailsDto>.Fail(payCoachingResult);

            var updateCoachingResult = await _userCoachingRepository.PayUserCoachingAsync(coaching.ToPaymentDao());
            if (!updateCoachingResult.Success)
                return ModelActionResult<CoachingDetailsDto>.Fail(updateCoachingResult);

            var updatedCoachingDao = updateCoachingResult.Results;
            return ModelActionResult<CoachingDetailsDto>.Ok(updatedCoachingDao.ToDetailsDto());
        }
    }
}

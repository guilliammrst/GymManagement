using GymManagement.Application.Interfaces.Repositories.Clubs;
using GymManagement.Application.Interfaces.Repositories.MembershipPlans;
using GymManagement.Application.Interfaces.Repositories.Memberships;
using GymManagement.Application.Interfaces.Repositories.Users;
using GymManagement.Application.Interfaces.Services.Memberships;
using GymManagement.Application.Interfaces.Services.Users;
using GymManagement.Application.Services.Converters;
using GymManagement.Domain;
using GymManagement.Shared.Core.Enums;
using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Services.Users
{
    public class UserSubscriptionService(IUserSubscriptionRepository _userSubscriptionRepository,
        IUserQueryRepository _userQueryRepository,
        IMembershipPlanQueryRepository _membershipPlanQueryRepository,
        IClubQueryRepository _clubQueryRepository,
        IMembershipQueryRepository _membershipQueryRepository) : IUserSubscriptionService
    {
        public async Task<ModelActionResult<UserDetailsDto>> AddUserSubscriptionAsync(UserSubscribeDto userSubscribeDto)
        {
            var userResult = await _userQueryRepository.GetUserByIdAsync(userSubscribeDto.UserId);
            if (!userResult.Success)
                return ModelActionResult<UserDetailsDto>.Fail(userResult);

            var userDao = userResult.Results;
            var user = User.Build(userDao.Id, userDao.CreatedAt, userDao.UpdatedAt, userDao.Name, userDao.Surname, userDao.Birthdate,
                userDao.Password, userDao.Role, userDao.Email, userDao.PhoneNumber, userDao.Gender, userDao.Address.Country, userDao.Address.City,
                userDao.Address.Street, userDao.Address.PostalCode, userDao.Address.Number, userDao.Memberships.Build()).Results;

            var membershipPlanResult = await _membershipPlanQueryRepository.GetMembershipPlanByIdAsync(userSubscribeDto.MembershipPlanId);
            if (!membershipPlanResult.Success)
                return ModelActionResult<UserDetailsDto>.Fail(membershipPlanResult);

            var membershipPlanDao = membershipPlanResult.Results;
            var membershipPlan = MembershipPlan.Build(membershipPlanDao.Id, membershipPlanDao.CreatedAt, membershipPlanDao.UpdatedAt, membershipPlanDao.IsValid,
                membershipPlanDao.Description, membershipPlanDao.MembershipPlanType, membershipPlanDao.BasePrice, membershipPlanDao.YearlyDiscount, membershipPlanDao.RegistrationFees).Results;

            var clubResult = await _clubQueryRepository.GetClubByIdAsync(userSubscribeDto.HomeClubId);
            if (!clubResult.Success)
                return ModelActionResult<UserDetailsDto>.Fail(clubResult);

            var clubDao = clubResult.Results;
            var club = Club.Build(clubDao.Id, clubDao.CreatedAt, clubDao.UpdatedAt, clubDao.Name, clubDao.Address.Country,
                clubDao.Address.City, clubDao.Address.Street, clubDao.Address.PostalCode, clubDao.Address.Number).Results;

            var createMembershipResult = Membership.Create(userSubscribeDto.StartDate, userSubscribeDto.MembershipPeriod, userSubscribeDto.RenewWhenExpiry, membershipPlan, club);
            if (!createMembershipResult.Success)
                return ModelActionResult<UserDetailsDto>.Fail(createMembershipResult);

            var membership = createMembershipResult.Results;

            var addNewMembershipResult = user.AddNewMembership(membership);
            if (!addNewMembershipResult.Success)
                return ModelActionResult<UserDetailsDto>.Fail(addNewMembershipResult);

            var updateUserResult = await _userSubscriptionRepository.SubscribeUserAsync(membership.ToSubscribeDao(user.Id));
            if (!updateUserResult.Success)
                return ModelActionResult<UserDetailsDto>.Fail(updateUserResult);

            var updatedUserDao = updateUserResult.Results;
            return ModelActionResult<UserDetailsDto>.Ok(updatedUserDao.ToDetailsDto());
        }

        public async Task<ModelActionResult<MembershipDetailsDto>> PayUserSubscriptionAsync(UserPaymentDto userPaymentDto)
        {
            var membershipResult = await _membershipQueryRepository.GetMembershipByIdAsync(userPaymentDto.EntityId);
            if (!membershipResult.Success)
                return ModelActionResult<MembershipDetailsDto>.Fail(membershipResult);

            var membershipDao = membershipResult.Results;

            if (membershipDao.Member.Id != userPaymentDto.UserId)
                return ModelActionResult<MembershipDetailsDto>.Fail(GymFaultType.BadParameter, "Pay membership failed: membership's userId is not the same that the userId provided.");

            var paymentDetailDao = membershipDao.PaymentDetail;

            var paymentDetailResult = PaymentDetail.Build(paymentDetailDao.Id, paymentDetailDao.CreatedAt, paymentDetailDao.UpdatedAt, paymentDetailDao.Amount,
                paymentDetailDao.PaymentMethod, paymentDetailDao.PaymentStatus, paymentDetailDao.PaymentDate);
            if (!paymentDetailResult.Success)
                return ModelActionResult<MembershipDetailsDto>.Fail(paymentDetailResult);

            var membership = Membership.Build(membershipDao.Id, membershipDao.CreatedAt, membershipDao.UpdatedAt, membershipDao.StartDate,
                membershipDao.EndDate, membershipDao.IsActive, membershipDao.RenewWhenExpiry, membershipDao.IsExpired, paymentDetailResult.Results).Results;

            var payMembershipResult = membership.Pay(userPaymentDto.PaymentMethod);
            if (!payMembershipResult.Success)
                return ModelActionResult<MembershipDetailsDto>.Fail(payMembershipResult);

            var updateMembershipResult = await _userSubscriptionRepository.PayUserSubscriptionAsync(membership.ToPaymentDao());
            if (!updateMembershipResult.Success)
                return ModelActionResult<MembershipDetailsDto>.Fail(updateMembershipResult);

            var updatedMembershipDao = updateMembershipResult.Results;
            return ModelActionResult<MembershipDetailsDto>.Ok(updatedMembershipDao.ToDetailsDto());
        }
    }
}

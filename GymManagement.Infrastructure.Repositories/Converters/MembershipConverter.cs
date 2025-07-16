using GymManagement.Application.Interfaces.Repositories.Clubs;
using GymManagement.Application.Interfaces.Repositories.MembershipPlans;
using GymManagement.Application.Interfaces.Repositories.Memberships;
using GymManagement.Application.Interfaces.Repositories.PaymentDetails;
using GymManagement.Application.Interfaces.Repositories.Users;
using GymManagement.Infrastructure.Persistence.Models;
using GymManagement.Shared.Core.Enums;

namespace GymManagement.Infrastructure.Repositories.Converters
{
    public static class MembershipConverter
    {
        public static MembershipDao ToDao(this MembershipModel membership)
        {
            return new MembershipDao
            {
                Id = membership.Id,
                CreatedAt = membership.CreatedAt,
                UpdatedAt = membership.UpdatedAt,
                StartDate = membership.StartDate,
                EndDate = membership.EndDate,
                IsActive = membership.IsActive,
                RenewWhenExpiry = membership.RenewWhenExpiry,
                IsExpired = membership.IsExpired
            };
        }

        public static List<MembershipDao> ToDao(this IEnumerable<MembershipModel> membership)
        {
            return membership.Select(ToDao).ToList();
        }

        public static MembershipModel ToModel(this UserSubscribeDao userSubscribeDto, MembershipPlanModel membershipPlan, ClubModel club)
        {
            return new MembershipModel
            {
                StartDate = userSubscribeDto.StartDate,
                EndDate = userSubscribeDto.EndDate,
                IsActive = userSubscribeDto.IsActive,
                RenewWhenExpiry = userSubscribeDto.RenewWhenExpiry,
                MembershipPlan = membershipPlan,
                HomeClub = club,
                PaymentDetail = new PaymentDetailModel
                {
                    Amount = userSubscribeDto.Amount
                }
            };
        }

        public static MembershipDetailsDao ToDetailsDao(this MembershipModel membership)
        {
            return new MembershipDetailsDao
            {
                Id = membership.Id,
                CreatedAt = membership.CreatedAt,
                UpdatedAt = membership.UpdatedAt,
                StartDate = membership.StartDate,
                EndDate = membership.EndDate,
                IsActive = membership.IsActive,
                RenewWhenExpiry = membership.RenewWhenExpiry,
                IsExpired = membership.IsExpired,
                Member = membership.Member?.ToDao() ?? new UserDao(),
                HomeClub = membership.HomeClub?.ToDao() ?? new ClubDao(),
                MembershipPlan = membership.MembershipPlan?.ToDao() ?? new MembershipPlanDao(),
                PaymentDetail = membership.PaymentDetail?.ToDao() ?? new PaymentDetailDao()
            };
        }
    }
}

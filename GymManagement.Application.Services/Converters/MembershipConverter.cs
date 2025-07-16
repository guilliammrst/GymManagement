using GymManagement.Application.Interfaces.Repositories.Memberships;
using GymManagement.Application.Interfaces.Repositories.Users;
using GymManagement.Application.Interfaces.Services.Memberships;
using GymManagement.Domain;
using Microsoft.AspNetCore.Http;

namespace GymManagement.Application.Services.Converters
{
    public static class MembershipConverter
    {
        public static MembershipDto ToDto(this MembershipDao membership)
        {
            return new MembershipDto
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

        public static List<MembershipDto> ToDto(this IEnumerable<MembershipDao> memberships)
        {
            return memberships.Select(ToDto).ToList();
        }

        public static Membership Build(this MembershipDao membership)
        {
            return Membership.Build(membership.Id, membership.CreatedAt, membership.UpdatedAt, membership.StartDate, membership.EndDate,
                membership.IsActive, membership.RenewWhenExpiry, membership.IsExpired).Results;
        }

        public static List<Membership> Build(this IEnumerable<MembershipDao> memberships)
        {
            return memberships.Select(Build).ToList();
        }

        public static UserSubscribeDao ToSubscribeDao(this Membership membership, int userId)
        {
            return new UserSubscribeDao
            {
                UserId = userId,
                StartDate = membership.StartDate,
                EndDate = membership.EndDate,
                IsActive = membership.IsActive,
                RenewWhenExpiry = membership.RenewWhenExpiry,
                MembershipPlanId = membership.MembershipPlan?.Id ?? 0,
                HomeClubId = membership.HomeClub?.Id ?? 0,
                Amount = membership.PaymentDetail?.Amount ?? 0,
            };
        }

        public static UserPaymentDao ToPaymentDao(this Membership membership)
        {
            return new UserPaymentDao
            {
                EntityId = membership.Id,
                PaymentMethod = membership.PaymentDetail!.PaymentMethod,
                PaymentStatus = membership.PaymentDetail.PaymentStatus,
                PaymentDate = membership.PaymentDetail.PaymentDate!.Value,
                IsActive = membership.IsActive
            };
        }

        public static MembershipDetailsDto ToDetailsDto(this MembershipDetailsDao membership)
        {
            return new MembershipDetailsDto
            {
                Id = membership.Id,
                CreatedAt = membership.CreatedAt,
                UpdatedAt = membership.UpdatedAt,
                StartDate = membership.StartDate,
                EndDate = membership.EndDate,
                IsActive = membership.IsActive,
                RenewWhenExpiry = membership.RenewWhenExpiry,
                IsExpired = membership.IsExpired,
                Member = membership.Member.ToDto(),
                HomeClub = membership.HomeClub.ToDto(),
                MembershipPlan = membership.MembershipPlan.ToDto(),
                PaymentDetail = membership.PaymentDetail.ToDto()
            };
        }
    }
}

using GymManagement.Application.Interfaces.Repositories.Coachings;
using GymManagement.Application.Interfaces.Repositories.Users;
using GymManagement.Application.Interfaces.Services.Coachings;
using GymManagement.Domain;

namespace GymManagement.Application.Services.Converters
{
    public static class CoachingConverter
    {
        public static UserPaymentDao ToPaymentDao(this Coaching coaching)
        {
            return new UserPaymentDao
            {
                EntityId = coaching.Id,
                PaymentMethod = coaching.PaymentDetail!.PaymentMethod,
                PaymentStatus = coaching.PaymentDetail.PaymentStatus,
                PaymentDate = coaching.PaymentDetail.PaymentDate!.Value,
                IsActive = coaching.IsActive
            };
        }

        public static UserCoachingDao ToUserCoachingDao(this Coaching coaching)
        {
            return new UserCoachingDao
            {
                StartDate = coaching.StartDate,
                EndDate = coaching.EndDate,
                IsActive = coaching.IsActive,
                RenewWhenExpiry = coaching.RenewWhenExpiry,
                WeekDay = coaching.WeekDay,
                Hour = coaching.Hour,
                Amount = coaching.PaymentDetail?.Amount ?? 0,
                MemberId = coaching.Member?.Id ?? 0,
                CoachingPlanId = coaching.CoachingPlan?.Id ?? 0,
            };
        }

        public static CoachingDetailsDto ToDetailsDto(this CoachingDetailsDao coaching)
        {
            return new CoachingDetailsDto
            {
                Id = coaching.Id,
                CreatedAt = coaching.CreatedAt,
                UpdatedAt = coaching.UpdatedAt,
                StartDate = coaching.StartDate,
                EndDate = coaching.EndDate,
                IsActive = coaching.IsActive,
                RenewWhenExpiry = coaching.RenewWhenExpiry,
                WeekDay = coaching.WeekDay,
                Hour = coaching.Hour,
                Member = coaching.Member.ToDto(),
                CoachingPlan = coaching.CoachingPlan.ToDto(),
                PaymentDetail = coaching.PaymentDetail.ToDto()
            };
        }

        public static CoachingDto ToDto(this CoachingDao coaching)
        {
            return new CoachingDto
            {
                Id = coaching.Id,
                CreatedAt = coaching.CreatedAt,
                UpdatedAt = coaching.UpdatedAt,
                StartDate = coaching.StartDate,
                EndDate = coaching.EndDate,
                IsActive = coaching.IsActive,
                RenewWhenExpiry = coaching.RenewWhenExpiry,
                WeekDay = coaching.WeekDay,
                Hour = coaching.Hour
            };
        }

        public static List<CoachingDto> ToDto(this IEnumerable<CoachingDao> coachings)
        {
            return coachings.Select(ToDto).ToList();
        }
    }
}

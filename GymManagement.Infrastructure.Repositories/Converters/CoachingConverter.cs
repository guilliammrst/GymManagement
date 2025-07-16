using GymManagement.Application.Interfaces.Repositories.Coachings;
using GymManagement.Application.Interfaces.Repositories.Users;
using GymManagement.Infrastructure.Persistence.Models;

namespace GymManagement.Infrastructure.Repositories.Converters
{
    public static class CoachingConverter
    {
        public static CoachingModel ToModel(this UserCoachingDao userCoaching)
        {
            return new CoachingModel
            {
                StartDate = userCoaching.StartDate,
                EndDate = userCoaching.EndDate,
                IsActive = userCoaching.IsActive,
                RenewWhenExpiry = userCoaching.RenewWhenExpiry,
                WeekDay = userCoaching.WeekDay,
                Hour = userCoaching.Hour,
                PaymentDetail = new PaymentDetailModel
                {
                    Amount = userCoaching.Amount
                }
            };
        }

        public static CoachingDetailsDao ToDetailsDao(this CoachingModel coachingModel)
        {
            return new CoachingDetailsDao
            {
                Id = coachingModel.Id,
                CreatedAt = coachingModel.CreatedAt,
                UpdatedAt = coachingModel.UpdatedAt,
                StartDate = coachingModel.StartDate,
                EndDate = coachingModel.EndDate,
                IsActive = coachingModel.IsActive,
                RenewWhenExpiry = coachingModel.RenewWhenExpiry,
                WeekDay = coachingModel.WeekDay,
                Hour = coachingModel.Hour,
                Member = coachingModel.Member?.ToDao() ?? new (),
                CoachingPlan = coachingModel.CoachingPlan?.ToDao() ?? new(),
                PaymentDetail = coachingModel.PaymentDetail?.ToDao() ?? new()
            };
        }

        public static CoachingDao ToDao(this CoachingModel coachingModel)
        {
            return new CoachingDao
            {
                Id = coachingModel.Id,
                CreatedAt = coachingModel.CreatedAt,
                UpdatedAt = coachingModel.UpdatedAt,
                StartDate = coachingModel.StartDate,
                EndDate = coachingModel.EndDate,
                IsActive = coachingModel.IsActive,
                RenewWhenExpiry = coachingModel.RenewWhenExpiry,
                WeekDay = coachingModel.WeekDay,
                Hour = coachingModel.Hour
            };
        }

        public static List<CoachingDao> ToDao(this IEnumerable<CoachingModel> coachingModels)
        {
            return coachingModels.Select(ToDao).ToList();
        }
    }
}

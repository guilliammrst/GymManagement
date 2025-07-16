using GymManagement.Application.Interfaces.Repositories.Coachings;
using GymManagement.Infrastructure.Persistence.Models;

namespace GymManagement.Infrastructure.Repositories.Converters
{
    public static class CoachingConverter
    {
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

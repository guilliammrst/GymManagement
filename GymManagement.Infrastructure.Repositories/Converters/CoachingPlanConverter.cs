using GymManagement.Application.Interfaces.Repositories.CoachingPlans;
using GymManagement.Infrastructure.Persistence.Models;

namespace GymManagement.Infrastructure.Repositories.Converters
{
    public static class CoachingPlanConverter
    {
        public static CoachingPlanModel ToModel(this CoachingPlanCreateDao coachingPlanCreateDao)
        {
            return new CoachingPlanModel
            {
                Description = coachingPlanCreateDao.Description,
                Price = coachingPlanCreateDao.Price,
                IsValid = coachingPlanCreateDao.IsValid,
            };
        }

        public static CoachingPlanDao ToDao(this CoachingPlanModel coachingPlan)
        {
            return new CoachingPlanDao
            {
                Id = coachingPlan.Id,
                CreatedAt = coachingPlan.CreatedAt,
                UpdatedAt = coachingPlan.UpdatedAt,
                Description = coachingPlan.Description,
                Price = coachingPlan.Price,
                IsValid = coachingPlan.IsValid,
            };
        }

        public static List<CoachingPlanDao> ToDao(this IEnumerable<CoachingPlanModel> coachingPlans)
        {
            return coachingPlans.Select(ToDao).ToList();
        }

        public static CoachingPlanDetailsDao ToDetailsDao(this CoachingPlanModel coachingPlan)
        {
            return new CoachingPlanDetailsDao
            {
                Id = coachingPlan.Id,
                CreatedAt = coachingPlan.CreatedAt,
                UpdatedAt = coachingPlan.UpdatedAt,
                Description = coachingPlan.Description,
                Price = coachingPlan.Price,
                IsValid = coachingPlan.IsValid,
                Coach = coachingPlan.Coach?.ToDao() ?? new (),
                Club = coachingPlan.Club?.ToDao() ?? new (),
                Coachings = coachingPlan.Coachings.ToDao() ?? []
            };
        }
    }
}

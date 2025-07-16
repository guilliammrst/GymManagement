using GymManagement.Application.Interfaces.Repositories.CoachingPlans;
using GymManagement.Application.Interfaces.Services.CoachingPlans;
using GymManagement.Domain;

namespace GymManagement.Application.Services.Converters
{
    public static class CoachingPlanConverter
    {
        public static CoachingPlanCreateDao ToCreateDao(this CoachingPlan coachingPlan)
        {
            return new CoachingPlanCreateDao
            {
                Description = coachingPlan.Description,
                Price = coachingPlan.Price,
                IsValid = coachingPlan.IsValid,
                CoachId = coachingPlan.CoachId!.Value,
                ClubId = coachingPlan.ClubId!.Value
            };
        }

        public static CoachingPlanDto ToDto(this CoachingPlanDao coachingPlan)
        {
            return new CoachingPlanDto
            {
                Id = coachingPlan.Id,
                CreatedAt = coachingPlan.CreatedAt,
                UpdatedAt = coachingPlan.UpdatedAt,
                Description = coachingPlan.Description,
                Price = coachingPlan.Price,
                IsValid = coachingPlan.IsValid,
            };
        }

        public static List<CoachingPlanDto> ToDto(this IEnumerable<CoachingPlanDao> coachingPlans)
        {
            return coachingPlans.Select(ToDto).ToList();
        }

        public static CoachingPlanDetailsDto ToDetailsDto(this CoachingPlanDetailsDao coachingPlan)
        {
            return new CoachingPlanDetailsDto
            {
                Id = coachingPlan.Id,
                CreatedAt = coachingPlan.CreatedAt,
                UpdatedAt = coachingPlan.UpdatedAt,
                Description = coachingPlan.Description,
                Price = coachingPlan.Price,
                IsValid = coachingPlan.IsValid,
                Coach = coachingPlan.Coach.ToDto(),
                Club = coachingPlan.Club.ToDto(),
                Coachings = coachingPlan.Coachings.ToDto()
            };
        }
    }
}

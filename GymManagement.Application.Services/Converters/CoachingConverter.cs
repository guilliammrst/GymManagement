using GymManagement.Application.Interfaces.Repositories.Coachings;
using GymManagement.Application.Interfaces.Services.Coachings;

namespace GymManagement.Application.Services.Converters
{
    public static class CoachingConverter
    {
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

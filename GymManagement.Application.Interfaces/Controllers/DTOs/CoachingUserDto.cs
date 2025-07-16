using GymManagement.Shared.Core.Enums;

namespace GymManagement.Application.Interfaces.Controllers.DTOs
{
    public class CoachingUserDto
    {
        public int? CoachingPlanId { get; set; }
        public DateTime? StartDate { get; set; }
        public bool? RenewWhenExpiry { get; set; }
        public WeekDays? WeekDay { get; set; }
        public int? Hour { get; set; }
    }
}

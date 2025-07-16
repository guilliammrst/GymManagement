using GymManagement.Shared.Core.BaseObjects;
using GymManagement.Shared.Core.Enums;

namespace GymManagement.Application.Interfaces.Services.Coachings
{
    public class CoachingDto : BaseDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool RenewWhenExpiry { get; set; }
        public WeekDays WeekDay { get; set; }
        public int Hour { get; set; }
    }
}

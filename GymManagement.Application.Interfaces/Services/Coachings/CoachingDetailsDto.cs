using GymManagement.Application.Interfaces.Services.CoachingPlans;
using GymManagement.Application.Interfaces.Services.PaymentDetails;
using GymManagement.Application.Interfaces.Services.Users;
using GymManagement.Shared.Core.BaseObjects;
using GymManagement.Shared.Core.Enums;

namespace GymManagement.Application.Interfaces.Services.Coachings
{
    public class CoachingDetailsDto : BaseDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool RenewWhenExpiry { get; set; }
        public WeekDays WeekDay { get; set; }
        public int Hour { get; set; }
        public UserDto Member { get; set; } = new();
        public CoachingPlanDto CoachingPlan { get; set; } = new();
        public PaymentDetailDto PaymentDetail { get; set; } = new();
    }
}

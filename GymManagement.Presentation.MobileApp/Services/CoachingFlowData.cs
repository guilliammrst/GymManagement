using GymManagement.Application.Interfaces.Services.CoachingPlans;
using GymManagement.Shared.Core.Enums;

namespace GymManagement.Presentation.MobileApp.Services
{
    public class CoachingFlowData
    {
        public int UserId { get; set; }
        public CoachingPlanDto CoachingPlan { get; set; }
        public DateTime StartDate { get; set; }
        public bool RenewWhenExpiry { get; set; }
        public WeekDays WeekDay { get; set; }
        public int Hour { get; set; }
    }
}

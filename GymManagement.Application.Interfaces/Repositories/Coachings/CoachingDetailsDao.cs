using GymManagement.Application.Interfaces.Repositories.CoachingPlans;
using GymManagement.Application.Interfaces.Repositories.PaymentDetails;
using GymManagement.Application.Interfaces.Repositories.Users;
using GymManagement.Shared.Core.BaseObjects;
using GymManagement.Shared.Core.Enums;

namespace GymManagement.Application.Interfaces.Repositories.Coachings
{
    public class CoachingDetailsDao : BaseDao
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool RenewWhenExpiry { get; set; }
        public WeekDays WeekDay { get; set; }
        public int Hour { get; set; }
        public UserDao Member { get; set; } = new ();
        public CoachingPlanDao CoachingPlan { get; set; } = new ();
        public PaymentDetailDao PaymentDetail { get; set; } = new ();
    }
}

using GymManagement.Shared.Core.Enums;

namespace GymManagement.Application.Interfaces.Repositories.Users
{
    public class UserCoachingDao
    {
        public required int MemberId { get; set; }
        public required int CoachingPlanId { get; set; }
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public required bool IsActive { get; set; }
        public required bool RenewWhenExpiry { get; set; }
        public required WeekDays WeekDay { get; set; }
        public required int Hour { get; set; }
        public required decimal Amount { get; set; }
    }
}
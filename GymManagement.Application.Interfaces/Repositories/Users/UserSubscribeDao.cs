namespace GymManagement.Application.Interfaces.Repositories.Users
{
    public class UserSubscribeDao
    {
        public required int UserId { get; set; }
        public required int HomeClubId { get; set; }
        public required int MembershipPlanId { get; set; }
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public required bool RenewWhenExpiry { get; set; }
        public required bool IsActive { get; set; }
        public required decimal Amount { get; set; }
    }
}

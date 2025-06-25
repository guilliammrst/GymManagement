using GymManagement.Shared.Core.Enums;

namespace GymManagement.Application.Interfaces.Repositories.MembershipPlans
{
    public class MembershipPlanUpdateDao
    {
        public required int Id { get; set; }
        public string? Description { get; set; }
        public MembershipPlanType? MembershipPlanType { get; set; }
        public decimal? BasePrice { get; set; }
        public decimal? YearlyDiscount { get; set; }
        public decimal? RegistrationFees { get; set; }
        public bool? IsValid { get; set; }
    }
}

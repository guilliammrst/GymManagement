using GymManagement.Shared.Core.Enums;

namespace GymManagement.Application.Interfaces.Repositories.MembershipPlans
{
    public class MembershipPlanCreateDao
    {
        public required string Description { get; set; }
        public required MembershipPlanType MembershipPlanType { get; set; }
        public required decimal BasePrice { get; set; }
        public required decimal YearlyDiscount { get; set; }
        public required decimal RegistrationFees { get; set; }
        public required bool IsValid { get; set; }
    }
}

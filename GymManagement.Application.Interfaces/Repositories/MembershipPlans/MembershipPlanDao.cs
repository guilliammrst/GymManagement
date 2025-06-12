using GymManagement.Shared.Core.BaseObjects;
using GymManagement.Shared.Core.Enums;

namespace GymManagement.Application.Interfaces.Repositories.MembershipPlans
{
    public class MembershipPlanDao : BaseDao
    {
        public string Description { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public bool IsValid { get; set; }
        public MembershipPlanType MembershipPlanType { get; set; }
        public decimal YearlyDiscount { get; set; }
        public decimal RegistrationFees { get; set; }
    }
}

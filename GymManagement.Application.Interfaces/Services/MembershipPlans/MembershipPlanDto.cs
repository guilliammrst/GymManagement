using GymManagement.Shared.Core.BaseObjects;
using GymManagement.Shared.Core.Enums;

namespace GymManagement.Application.Interfaces.Services.MembershipPlans
{
    public class MembershipPlanDto : BaseDto
    {
        public string Description { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public bool IsValid { get; set; }
        public MembershipPlanType MembershipPlanType { get; set; }
        public decimal YearlyDiscount { get; set; }
        public decimal RegistrationFees { get; set; }
    }
}

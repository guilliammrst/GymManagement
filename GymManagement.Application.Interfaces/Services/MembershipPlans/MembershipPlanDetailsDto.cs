using GymManagement.Application.Interfaces.Services.Memberships;
using GymManagement.Shared.Core.BaseObjects;
using GymManagement.Shared.Core.Enums;

namespace GymManagement.Application.Interfaces.Services.MembershipPlans
{
    public class MembershipPlanDetailsDto : BaseDto
    {
        public string Description { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public bool IsValid { get; set; }
        public MembershipPlanType MembershipPlanType { get; set; }
        public decimal YearlyDiscount { get; set; }
        public decimal RegistrationFees { get; set; }
        public List<MembershipDto> Memberships { get; set; }
    }
}

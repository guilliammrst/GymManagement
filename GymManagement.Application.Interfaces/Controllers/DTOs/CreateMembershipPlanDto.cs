using GymManagement.Shared.Core.Enums;

namespace GymManagement.Application.Interfaces.Controllers.DTOs
{
    public class CreateMembershipPlanDto
    {
        public string? Description { get; set; }
        public MembershipPlanType? MembershipPlanType { get; set; }
        public decimal? BasePrice { get; set; }
        public decimal? YearlyDiscount { get; set; }
        public decimal? RegistrationFees { get; set; }
    }
}

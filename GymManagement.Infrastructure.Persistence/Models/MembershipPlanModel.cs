using System.ComponentModel.DataAnnotations.Schema;
using GymManagement.Shared.Core.Enums;

namespace GymManagement.Infrastructure.Persistence.Models
{
    [Table("membership_plans")]
    public class MembershipPlanModel : BaseModel
    {
        [Column("description")]
        public string Description { get; set; } = string.Empty;

        [Column("membership_plan_type")]
        public MembershipPlanType MembershipPlanType { get; set; } = MembershipPlanType.None;

        [Column("base_price")]
        public decimal BasePrice { get; set; }

        [Column("yearly_discount")]
        public decimal YearlyDiscount { get; set; }

        [Column("registration_fees")]
        public decimal RegistrationFees { get; set; }

        [Column("is_valid")]
        public bool IsValid { get; set; } = false;

        public ICollection<MembershipModel> Memberships { get; set; } = [];
    }
}

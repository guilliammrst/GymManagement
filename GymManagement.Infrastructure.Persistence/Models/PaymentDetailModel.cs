using System.ComponentModel.DataAnnotations.Schema;
using GymManagement.Shared.Core.Enums;

namespace GymManagement.Infrastructure.Persistence.Models
{
    [Table("payment_details")]
    public class PaymentDetailModel : BaseModel
    {
        [Column("payment_method")]
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.None;

        [Column("payment_date")]
        public DateTime? PaymentDate { get; set; }

        [Column("amount")]
        public decimal Amount { get; set; }

        [Column("payment_status")]
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.None;

        public MembershipModel? Membership { get; set; }
    }
}

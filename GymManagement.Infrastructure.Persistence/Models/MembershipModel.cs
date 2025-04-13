using System.ComponentModel.DataAnnotations.Schema;

namespace GymManagement.Infrastructure.Persistence.Models
{
    [Table("memberships")]
    public class MembershipModel : BaseModel
    {
        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Column("end_date")]
        public DateTime EndDate { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = false;

        [Column("renew_when_expiry")]
        public bool RenewWhenExpiry { get; set; } = false;

        [NotMapped]
        public bool IsExpired => EndDate < DateTime.UtcNow;

        [Column("member_id")]
        public int MemberId { get; set; }
        public UserModel? Member { get; set; }

        [Column("home_club_id")]
        public int HomeClubId { get; set; }
        public ClubModel? HomeClub { get; set; }

        [Column("membership_plan_id")]
        public int MembershipPlanId { get; set; }
        public MembershipPlanModel? MembershipPlan { get; set; }

        [Column("payment_detail_id")]
        public int PaymentDetailId { get; set; }
        public PaymentDetailModel? PaymentDetail { get; set; }
    }
}

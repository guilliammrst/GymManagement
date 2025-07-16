using GymManagement.Shared.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymManagement.Infrastructure.Persistence.Models
{
    [Table("coachings")]
    public class CoachingModel : BaseModel
    {
        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Column("end_date")]
        public DateTime EndDate { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = false;

        [Column("renew_when_expiry")]
        public bool RenewWhenExpiry { get; set; } = false;

        [Column("week_day")]
        public WeekDays WeekDay { get; set; }

        [Column("hour")]
        public int Hour { get; set; }

        [Column("member_id")]
        public int MemberId { get; set; }
        public UserModel? Member { get; set; }

        [Column("coaching_plan_id")]
        public int CoachingPlanId { get; set; }
        public CoachingPlanModel? CoachingPlan { get; set; }

        [Column("payment_detail_id")]
        public int PaymentDetailId { get; set; }
        public PaymentDetailModel? PaymentDetail { get; set; }
    }
}

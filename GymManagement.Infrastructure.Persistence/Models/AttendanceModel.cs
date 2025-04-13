using System.ComponentModel.DataAnnotations.Schema;

namespace GymManagement.Infrastructure.Persistence.Models
{
    [Table("attendances")]
    public class AttendanceModel : BaseModel
    {
        [Column("check_in_time")]
        public DateTime CheckInTime { get; set; } = DateTime.UtcNow;

        [Column("member_id")]
        public int MemberId { get; set; }
        public UserModel? Member { get; set; }

        [Column("club_id")]
        public int ClubId { get; set; }
        public ClubModel? Club { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace GymManagement.Infrastructure.Persistence.Models
{
    [Table("clubs")]
    public class ClubModel : BaseModel
    {
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("address_id")]
        public int AddressId { get; set; }
        public AddressModel? Address { get; set; }

        [Column("manager_id")]
        public int? ManagerId { get; set; }
        public UserModel? Manager { get; set; }

        public ICollection<MembershipModel> Memberships { get; set; } = [];

        public ICollection<AttendanceModel> Attendances { get; set; } = [];

        public ICollection<CoachingPlanModel> CoachingPlans { get; set; } = [];
    }
}

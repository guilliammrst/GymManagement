using System.ComponentModel.DataAnnotations.Schema;
using GymManagement.Shared.Core.Enums;

namespace GymManagement.Infrastructure.Persistence.Models
{
    [Table("users")]
    public class UserModel : BaseModel
    {
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("surname")]
        public string Surname { get; set; } = string.Empty;

        [Column("birthdate")]
        public DateTime Birthdate { get; set; }

        [Column("password")]
        public string Password { get; set; } = string.Empty;

        [Column("role")]
        public Role Role { get; set; } = Role.None;

        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Column("phone_number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Column("gender")]
        public Gender Gender { get; set; } = Gender.None;

        [Column("address_id")]
        public int? AddressId { get; set; }
        public AddressModel? Address { get; set; }

        public ICollection<MembershipModel> Memberships { get; set; } = [];

        public ICollection<AttendanceModel> Attendances { get; set; } = [];
    }
}

using GymManagement.Application.Interfaces.Repositories.Addresses;
using GymManagement.Application.Interfaces.Repositories.Attendances;
using GymManagement.Application.Interfaces.Repositories.Memberships;
using GymManagement.Application.Interfaces.Repositories.Users;
using GymManagement.Shared.Core.BaseObjects;

namespace GymManagement.Application.Interfaces.Repositories.Clubs
{
    public class ClubDetailsDao : BaseDao
    {
        public string Name { get; set; } = string.Empty;
        public AddressDao Address { get; set; }
        public List<AttendanceDao> Attendances { get; set; }
        public List<MembershipDao> Memberships { get; set; }
        public UserDao Manager { get; set; }
    }
}

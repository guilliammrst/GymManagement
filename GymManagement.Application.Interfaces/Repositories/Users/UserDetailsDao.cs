using GymManagement.Application.Interfaces.Repositories.Addresses;
using GymManagement.Application.Interfaces.Repositories.Attendances;
using GymManagement.Application.Interfaces.Repositories.Memberships;

namespace GymManagement.Application.Interfaces.Repositories.Users
{
    public class UserDetailsDao : UserDao
    {
        public AddressDao Address { get; set; }
        public List <MembershipDao> Memberships { get; set; }

        public List<AttendanceDao> Attendances{ get; set; }
    }
}

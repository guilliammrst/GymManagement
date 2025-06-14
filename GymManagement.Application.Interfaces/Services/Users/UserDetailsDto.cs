using GymManagement.Application.Interfaces.Services.Attendances;
using GymManagement.Application.Interfaces.Services.Memberships;


namespace GymManagement.Application.Interfaces.Services.Users
{
    public class UserDetailsDto : UserDto
    {
        public List<MembershipDto> Memberships { get; set; }
        public List<AttendanceDto> Attendances { get; set; }
    }
}

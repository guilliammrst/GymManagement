using GymManagement.Application.Interfaces.Services.Attendances;
using GymManagement.Application.Interfaces.Services.Memberships;
using GymManagement.Application.Interfaces.Services.Users;

namespace GymManagement.Application.Interfaces.Services.Clubs
{
    public class ClubDetailsDto : ClubDto
    {
        public List<AttendanceDto> Attendances { get; set; }
        public List<MembershipDto> Memberships { get; set; }
        public UserDto Manager { get; set; }
    }
}

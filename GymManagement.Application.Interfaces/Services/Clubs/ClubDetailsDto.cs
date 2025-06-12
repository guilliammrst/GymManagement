using GymManagement.Application.Interfaces.Services.Addresses;
using GymManagement.Application.Interfaces.Services.Attendances;
using GymManagement.Application.Interfaces.Services.Memberships;
using GymManagement.Application.Interfaces.Services.Users;
using GymManagement.Shared.Core.BaseObjects;

namespace GymManagement.Application.Interfaces.Services.Clubs
{
    public class ClubDetailsDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public AddressDto Address { get; set; }
        public List<AttendanceDto> Attendances { get; set; }
        public List<MembershipDto> Memberships { get; set; }
        public UserDto Manager { get; set; }
    }
}

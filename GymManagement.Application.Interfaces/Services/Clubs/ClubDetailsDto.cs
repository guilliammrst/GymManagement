using GymManagement.Application.Interfaces.Services.Attendances;
using GymManagement.Application.Interfaces.Services.Memberships;
using GymManagement.Application.Interfaces.Services.Users;
using GymManagement.Application.Interfaces.Services.Addresses;

namespace GymManagement.Application.Interfaces.Services.Clubs
{
    public class ClubDetailsDto : ClubDto
    {
        public List<AttendanceDto> Attendances { get; set; }
        public List<MembershipDto> Memberships { get; set; }
        public UserDto Manager { get; set; }
        
        // Convenience properties for Blazor components
        public AddressDto AddressDto => Address;
        public string Description => $"Club {Name}"; // Default description
    }
}

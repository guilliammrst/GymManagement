using GymManagement.Application.Interfaces.Services.Addresses;
using GymManagement.Shared.Core.BaseObjects;

namespace GymManagement.Application.Interfaces.Services.Clubs
{
    public class ClubDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public AddressDto Address { get; set; }
    }
}

using GymManagement.Application.Interfaces.Services.Addresses;
using GymManagement.Shared.Core.BaseObjects;

namespace GymManagement.Application.Interfaces.Services.Clubs
{
    public class ClubCreateResult : BaseDao
    {
        public string Name { get; set; } = string.Empty;
        public AddressDto Address { get; set; }
        public int? ManagerId { get; set; }
    }
}

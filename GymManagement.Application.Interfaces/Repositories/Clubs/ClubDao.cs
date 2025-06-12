using GymManagement.Application.Interfaces.Repositories.Addresses;
using GymManagement.Shared.Core.BaseObjects;

namespace GymManagement.Application.Interfaces.Repositories.Clubs
{
    public class ClubDao : BaseDao
    {
        public string Name { get; set; } = string.Empty;
        public AddressDao Address { get; set; }
    }
}

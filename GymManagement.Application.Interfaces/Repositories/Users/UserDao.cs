using GymManagement.Application.Interfaces.Repositories.Addresses;
using GymManagement.Shared.Core.BaseObjects;
using GymManagement.Shared.Core.Enums;

namespace GymManagement.Application.Interfaces.Repositories.Users
{
    public class UserDao : BaseDao
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public DateTime Birthdate { get; set; }
        public string Password { get; set; } = string.Empty;
        public Role Role { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public Gender Gender { get; set; }
        public AddressDao Address { get; set; } 
    }
}

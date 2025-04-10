using GymManagement.Shared.Core.Enums;

namespace GymManagement.Application.Interfaces.Repositories.Users
{
    public class UserCreateDao
    {
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required DateTime Birthdate { get; set; }
        public required string Password { get; set; }
        public required Role Role { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required Gender Gender { get; set; }
    }
}

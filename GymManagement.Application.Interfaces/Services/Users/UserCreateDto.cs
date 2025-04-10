using GymManagement.Shared.Core.Enums;

namespace GymManagement.Application.Interfaces.Services.Users
{
    public class UserCreateDto
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public DateTime? Birthdate { get; set; }
        public string? Password { get; set; }
        public Role? Role { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public Gender? Gender { get; set; }
    }
}

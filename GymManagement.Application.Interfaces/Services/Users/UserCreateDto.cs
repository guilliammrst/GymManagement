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
        public Country? Country { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? PostalCode { get; set; }
        public string? Number { get; set; }
    }
}

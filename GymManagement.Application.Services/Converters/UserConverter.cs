using GymManagement.Domain;
using GymManagement.Application.Interfaces.Services.Users;
using GymManagement.Application.Interfaces.Repositories.Users;

namespace GymManagement.Application.Services.Converters
{
    public static class UserConverter
    {
        public static UserUpdateDao ToUpdateDao(this User user)
        {
            return new UserUpdateDao
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Birthdate = user.Birthdate,
                Password = user.Password,
                Role = user.Role,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Gender = user.Gender,
                Country = user.Country,
                City = user.City,
                PostalCode = user.PostalCode,
                Street = user.Street,
                Number = user.Number
            };
        }

        public static UserCreateDao ToCreateDao(this User user)
        {
            return new UserCreateDao
            {
                Name = user.Name,
                Surname = user.Surname,
                Birthdate = user.Birthdate,
                Password = user.Password,
                Role = user.Role,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Gender = user.Gender,
                Country = user.Country,
                City = user.City,
                PostalCode = user.PostalCode,
                Street = user.Street,
                Number = user.Number
            };
        }

        public static UserDto ToDto(this UserDao user)
        {
            return new UserDto
            {
                Id = user.Id,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                Name = user.Name,
                Surname = user.Surname,
                Birthdate = user.Birthdate,
                Password = user.Password,
                Role = user.Role,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Gender = user.Gender,
                Address = user.Address.ToDto()
            };
        }

        public static UserDetailsDto ToDetailsDto(this UserDetailsDao user)
        {
            return new UserDetailsDto
            {
                Id = user.Id,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                Name = user.Name,
                Surname = user.Surname,
                Birthdate = user.Birthdate,
                Password = user.Password,
                Role = user.Role,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Gender = user.Gender,
                Address = user.Address.ToDto(),
                Memberships = user.Memberships.ToDto(),
                Attendances = user.Attendances.ToDto()
            };
        }

        public static List<UserDto> ToDto(this List<UserDao> users)
        {
            return users.Select(ToDto).ToList();
        }
    }
}

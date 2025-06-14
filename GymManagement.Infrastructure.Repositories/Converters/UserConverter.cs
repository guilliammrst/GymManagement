using GymManagement.Application.Interfaces.Repositories.Addresses;
using GymManagement.Application.Interfaces.Repositories.Users;
using GymManagement.Infrastructure.Persistence.Models;

namespace GymManagement.Infrastructure.Repositories.Converters
{
    public static class UserConverter
    {
        public static UserModel ToModel(this UserCreateDao user)
        {
            return new UserModel
            {
                Name = user.Name,
                Surname = user.Surname,
                Birthdate = user.Birthdate,
                Password = user.Password,
                Role = user.Role,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Gender = user.Gender,
                Address = new AddressModel
                {
                    Country = user.Country,
                    City = user.City,
                    PostalCode = user.PostalCode,
                    Street = user.Street,
                    Number = user.Number
                }
            };
        }

        public static UserDao ToDao(this UserModel user)
        {
            return new UserDao
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
                Address = user.Address == null ? new AddressDao() : user.Address.ToDao()
            };
        }

        public static UserDetailsDao ToDetailsDao(this UserModel user)
        {
            return new UserDetailsDao
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
                Address = user.Address == null ? new AddressDao() : user.Address.ToDao(),
                Memberships = user.Memberships.ToDao(),
                Attendances = user.Attendances.ToDao()
            };
        }

        public static List<UserDao> ToDao(this IEnumerable<UserModel> user)
        {
            return user.Select(ToDao).ToList();
        }

        public static UserDetailsDao ToDetailsDao(this UserDao user)
        {
            return new UserDetailsDao
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
                Address = user.Address ?? new AddressDao()
            };
        }
    }
}

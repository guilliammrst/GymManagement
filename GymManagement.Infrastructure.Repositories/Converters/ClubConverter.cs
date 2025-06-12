using GymManagement.Application.Interfaces.Repositories.Addresses;
using GymManagement.Application.Interfaces.Repositories.Clubs;
using GymManagement.Application.Interfaces.Repositories.Users;
using GymManagement.Infrastructure.Persistence.Models;

namespace GymManagement.Infrastructure.Repositories.Converters
{
    public static class ClubConverter
    {
        public static ClubDetailsDao ToDetailsDao(this ClubModel club)
        {
            return new ClubDetailsDao
            {
                Id = club.Id,
                CreatedAt = club.CreatedAt,
                UpdatedAt = club.UpdatedAt,
                Name = club.Name,
                Address = club.Address == null ? new AddressDao() : club.Address.ToDao(),
                Manager = club.Manager == null ? new UserDao() : club.Manager.ToDao(),
                Attendances = club.Attendances.ToDao(),
                Memberships = club.Memberships.ToDao()
            };
        }

        public static ClubDao ToDao(this ClubModel club)
        {
            return new ClubDao
            {
                Id = club.Id,
                CreatedAt = club.CreatedAt,
                UpdatedAt = club.UpdatedAt,
                Name = club.Name,
                Address = club.Address == null ? new AddressDao() : club.Address.ToDao()
            };
        }

        public static List<ClubDao> ToDao(this IEnumerable<ClubModel> clubs)
        {
            return clubs.Select(ToDao).ToList();
        }

        public static ClubModel ToModel(this ClubCreateDao club)
        {
            return new ClubModel
            {
                Name = club.Name
            };
        }

        public static AddressCreateDao ToAddressCreate(this ClubCreateDao club)
        {
            return new AddressCreateDao
            {
                Country = club.Country,
                City = club.City,
                Street = club.Street,
                PostalCode = club.PostalCode,
                Number = club.Number
            };
        }
    }
}

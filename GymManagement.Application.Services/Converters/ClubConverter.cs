using GymManagement.Application.Interfaces.Repositories.Clubs;
using GymManagement.Application.Interfaces.Services.Clubs;
using GymManagement.Domain;

namespace GymManagement.Application.Services.Converters
{
    public static class ClubConverter
    {
        public static ClubDto ToDto(this ClubDao club)
        {
            return new ClubDto
            {
                Id = club.Id,
                CreatedAt = club.CreatedAt,
                UpdatedAt = club.UpdatedAt,
                Name = club.Name,
                Address = club.Address.ToDto()
            };
        }

        public static List<ClubDto> ToDto(this IEnumerable<ClubDao> clubs)
        {
            return clubs.Select(ToDto).ToList();
        }

        public static ClubDetailsDto ToDetailsDto(this ClubDetailsDao club)
        {
            return new ClubDetailsDto
            {
                Id = club.Id,
                CreatedAt = club.CreatedAt,
                UpdatedAt = club.UpdatedAt,
                Name = club.Name,
                Address = club.Address.ToDto(),
                Manager = club.Manager.ToDto(),
                Attendances = club.Attendances.ToDto(),
                Memberships = club.Memberships.ToDto()
            };
        }

        public static ClubCreateDao ToCreateDao(this Club club)
        {
            return new ClubCreateDao
            {
                Name = club.Name,
                Country = club.Country,
                City = club.City,
                Street = club.Street,
                PostalCode = club.PostalCode,
                Number = club.Number,
                ManagerId = club.ManagerId
            };
        }

        public static ClubCreateResult ToCreateResult(this ClubDetailsDao club)
        {
            return new ClubCreateResult
            {
                Id = club.Id,
                CreatedAt = club.CreatedAt,
                UpdatedAt = club.UpdatedAt,
                Name = club.Name,
                Address = club.Address.ToDto(),
                ManagerId = club.Manager.Id
            };
        }
    }
}

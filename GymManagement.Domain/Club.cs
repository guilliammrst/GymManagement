using GymManagement.Shared.Core.BaseObjects;
using GymManagement.Shared.Core.Enums;
using GymManagement.Shared.Core.Results;

namespace GymManagement.Domain
{
    public class Club : BaseObject
    {
        private Club(string name, Country country, string city, string street, string postalCode, string number, int? managerId) : base(0, DateTime.UtcNow)
        {
            Name = name;
            Country = country;
            City = city;
            Street = street;
            PostalCode = postalCode;
            Number = number;
            ManagerId = managerId;
        }

        public string Name { get; }
        public Country Country { get; }
        public string City { get; }
        public string Street { get; }
        public string PostalCode { get; }
        public string Number { get; }
        public int? ManagerId { get; }

        public static ModelActionResult<Club> Create(string? name, Country? country, string? city, string? street, string? postalCode, string? number, int? managerId)
        {
            if (string.IsNullOrWhiteSpace(name))
                return ModelActionResult<Club>.Fail(GymFaultType.BadParameter, "Club creation failed: field Name is required.");

            if (country is null)
                return ModelActionResult<Club>.Fail(GymFaultType.BadParameter, "Club creation failed: field Country is required.");

            if (!Enum.IsDefined((Country)country))
                return ModelActionResult<Club>.Fail(GymFaultType.BadParameter, "Club creation failed: field Country does not contain a valid value.");

            if (string.IsNullOrWhiteSpace(city))
                return ModelActionResult<Club>.Fail(GymFaultType.BadParameter, "Club creation failed: field City is required.");

            if (string.IsNullOrWhiteSpace(street))
                return ModelActionResult<Club>.Fail(GymFaultType.BadParameter, "Club creation failed: field Street is required.");

            if (string.IsNullOrWhiteSpace(postalCode))
                return ModelActionResult<Club>.Fail(GymFaultType.BadParameter, "Club creation failed: field PostalCode is required.");

            if (string.IsNullOrWhiteSpace(number))
                return ModelActionResult<Club>.Fail(GymFaultType.BadParameter, "Club creation failed: field Number is required.");

            if (managerId.HasValue && managerId <= 0)
                return ModelActionResult<Club>.Fail(GymFaultType.BadParameter, "Club creation failed: field ManagerId must be a positive integer or null.");

            return ModelActionResult<Club>.Ok(new Club(name, country.Value, city, street, postalCode, number, managerId));
        }
    }
}

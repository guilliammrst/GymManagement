﻿using GymManagement.Shared.Core.BaseObjects;
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

        private Club(int id, DateTime createdAt, DateTime? updatedAt, string name, Country country, string city, string street, string postalCode, string number, int? managerId) : base(id, createdAt, updatedAt)
        {
            Name = name;
            Country = country;
            City = city;
            Street = street;
            PostalCode = postalCode;
            Number = number;
            ManagerId = managerId;
        }

        public string Name { get; private set; }
        public Country Country { get; private set; }
        public string City { get; private set; }
        public string Street { get; private set; }
        public string PostalCode { get; private set; }
        public string Number { get; private set; }
        public int? ManagerId { get; private set; }

        public static ModelActionResult<Club> Create(string? name, Country? country, string? city, string? street, string? postalCode, string? number, int? managerId)
        {
            if (string.IsNullOrWhiteSpace(name))
                return ModelActionResult<Club>.Fail(GymFaultType.BadParameter, "Club creation failed: field Name is required.");

            if (!country.HasValue)
                return ModelActionResult<Club>.Fail(GymFaultType.BadParameter, "Club creation failed: field Country is required.");

            if (!Enum.IsDefined(country.Value))
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

        public static ModelActionResult<Club> Build(int id, DateTime createdAt, DateTime? updatedAt, string name, Country country, string city, string street, string postalCode, string number, int? managerId = null)
        {
            return ModelActionResult<Club>.Ok(new Club(id, createdAt, updatedAt, name, country, city, street, postalCode, number, managerId));
        }

        public ModelActionResult Update(string? name, Country? country, string? city, string? street, string? postalCode, string? number, int? managerId)
        {
            if (!string.IsNullOrWhiteSpace(name))
                Name = name;

            if (country.HasValue)
            {
                if (!Enum.IsDefined(country.Value))
                    return ModelActionResult.Fail(GymFaultType.BadParameter, "Club update failed: field Country does not contain a valid value.");

                Country = country.Value;
            }

            if (!string.IsNullOrWhiteSpace(city))
                City = city;

            if (!string.IsNullOrWhiteSpace(street))
                Street = street;

            if (!string.IsNullOrWhiteSpace(postalCode))
                PostalCode = postalCode;

            if (!string.IsNullOrWhiteSpace(number))
                Number = number;

            if (managerId.HasValue)
            {
                if (managerId <= 0)
                    return ModelActionResult.Fail(GymFaultType.BadParameter, "Club update failed: field ManagerId must be a positive integer or null.");

                ManagerId = managerId;
            }

            return ModelActionResult.Ok;
        }
    }
}

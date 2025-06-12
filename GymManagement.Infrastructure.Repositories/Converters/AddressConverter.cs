using GymManagement.Application.Interfaces.Repositories.Addresses;
using GymManagement.Infrastructure.Persistence.Models;

namespace GymManagement.Infrastructure.Repositories.Converters
{
    public static class AddressConverter
    {
        public static AddressDao ToDao(this AddressModel address)
        {
            return new AddressDao()
            {
                Id = address.Id,
                CreatedAt = address.CreatedAt,
                UpdatedAt = address.UpdatedAt,
                Country = address.Country,
                City = address.City,
                Street = address.Street,
                PostalCode = address.PostalCode,
                Number = address.Number
            };
        }

        public static AddressModel ToModel(this AddressCreateDao address)
        {
            return new AddressModel()
            {
                Country = address.Country,
                City = address.City,
                Street = address.Street,
                PostalCode = address.PostalCode,
                Number = address.Number
            };
        }
    }
}

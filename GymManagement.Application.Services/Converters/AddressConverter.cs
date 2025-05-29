using GymManagement.Application.Interfaces.Repositories.Addresses;
using GymManagement.Application.Interfaces.Services.Addresses;

namespace GymManagement.Application.Services.Converters
{
    public static class AddressConverter
    {
        public static AddressDto ToDto(this AddressDao address)
        {
            return new AddressDto
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
    }
}

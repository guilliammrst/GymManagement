using GymManagement.Shared.Core.BaseObjects;
using GymManagement.Shared.Core.Enums;

namespace GymManagement.Application.Interfaces.Services.Addresses
{
    public class AddressDto : BaseDto
    {
        public Country Country { get; set; }
        public string City { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
    }
}

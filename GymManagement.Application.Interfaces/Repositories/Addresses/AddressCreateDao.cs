using GymManagement.Shared.Core.Enums;

namespace GymManagement.Application.Interfaces.Repositories.Addresses
{
    public class AddressCreateDao
    {
        public required string Street { get; set; }
        public required string City { get; set; }
        public required string Number { get; set; }
        public required string PostalCode { get; set; }
        public required Country Country { get; set; }
    }
}

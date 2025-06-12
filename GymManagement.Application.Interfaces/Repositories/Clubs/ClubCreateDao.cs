using GymManagement.Shared.Core.Enums;

namespace GymManagement.Application.Interfaces.Repositories.Clubs
{
    public class ClubCreateDao
    {
        public required string Name { get; set; }
        public required Country Country { get; set; }
        public required string City { get; set; }
        public required string Street { get; set; }
        public required string PostalCode { get; set; }
        public required string Number { get; set; }
        public int? ManagerId { get; set; }
    }
}

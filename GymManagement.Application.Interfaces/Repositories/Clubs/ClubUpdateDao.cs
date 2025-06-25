using GymManagement.Shared.Core.Enums;

namespace GymManagement.Application.Interfaces.Repositories.Clubs
{
    public class ClubUpdateDao
    {
        public required int Id { get; set; }
        public string? Name { get; set; }
        public Country? Country { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? PostalCode { get; set; }
        public string? Number { get; set; }
        public int? ManagerId { get; set; }
    }
}

using GymManagement.Shared.Core.BaseObjects;

namespace GymManagement.Application.Interfaces.Repositories.Memberships
{
    public class MembershipDao : BaseDao
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool RenewWhenExpiry { get; set; }
        public bool IsExpired { get; set; }
    }
}

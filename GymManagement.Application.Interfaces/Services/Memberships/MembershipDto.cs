using GymManagement.Shared.Core.BaseObjects;

namespace GymManagement.Application.Interfaces.Services.Memberships
{
    public class MembershipDto : BaseDto
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; } = false;

        public bool RenewWhenExpiry { get; set; } = false;

        public bool IsExpired { get; set; }
    }
}

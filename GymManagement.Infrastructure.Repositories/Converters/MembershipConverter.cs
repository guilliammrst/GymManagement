using GymManagement.Application.Interfaces.Repositories.Memberships;
using GymManagement.Infrastructure.Persistence.Models;

namespace GymManagement.Infrastructure.Repositories.Converters
{
    public static class MembershipConverter
    {
        public static MembershipDao ToDao(this MembershipModel membership)
        {
            return new MembershipDao
            {
                Id = membership.Id,
                CreatedAt = membership.CreatedAt,
                UpdatedAt = membership.UpdatedAt,
                StartDate = membership.StartDate,
                EndDate = membership.EndDate,
                IsActive = membership.IsActive,
                RenewWhenExpiry = membership.RenewWhenExpiry,
                IsExpired = membership.IsExpired
            };
        }

        public static List<MembershipDao> ToDao(this IEnumerable<MembershipModel> membership)
        {
            return membership.Select(ToDao).ToList();
        }
    }
}

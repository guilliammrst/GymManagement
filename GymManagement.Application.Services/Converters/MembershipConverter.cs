using GymManagement.Application.Interfaces.Repositories.Memberships;
using GymManagement.Application.Interfaces.Services.Memberships;

namespace GymManagement.Application.Services.Converters
{
    public static class MembershipConverter
    {
        public static MembershipDto ToDto(this MembershipDao membership)
        {
            return new MembershipDto
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

        public static List<MembershipDto> ToDto(this IEnumerable<MembershipDao> memberships)
        {
            return memberships.Select(ToDto).ToList();
        }
    }
}

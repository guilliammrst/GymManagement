using GymManagement.Shared.Core.Enums;
using GymManagement.Shared.Core.Results;

namespace GymManagement.Shared.Core.Converters
{
    public static class DateConverter
    {
        public static ModelActionResult<DateTime> ToEndDate(this DateTime startDate, MembershipPeriod membershipPeriod)
        {
            return membershipPeriod switch
            {
                MembershipPeriod.FourWeeks => ModelActionResult<DateTime>.Ok(startDate.AddDays(4 * 7)),
                MembershipPeriod.OneYear => ModelActionResult<DateTime>.Ok(startDate.AddYears(1)),
                _ => ModelActionResult<DateTime>.Fail(GymFaultType.BadParameter, "Membership creation failed: field MembershipPeriod contains an invalid period.")
            };
        }
    }
}

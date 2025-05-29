

using GymManagement.Application.Interfaces.Repositories.Attendances;
using GymManagement.Application.Interfaces.Repositories.Memberships;
using GymManagement.Infrastructure.Persistence.Models;

namespace GymManagement.Infrastructure.Repositories.Converters
{
    public static class AttendanceConverter
    {

        public static AttendanceDao ToDao(this AttendanceModel attendance)
        {
            return new AttendanceDao
            {
                Id = attendance.Id,
                CreatedAt = attendance.CreatedAt,
                UpdatedAt = attendance.UpdatedAt,
                CheckInTime = attendance.CheckInTime
            };
        }

        public static List<AttendanceDao> ToDao(this IEnumerable<AttendanceModel> attendance)
        {
            return attendance.Select(ToDao).ToList();
        }
    }
}

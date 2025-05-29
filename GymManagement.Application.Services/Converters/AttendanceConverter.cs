using GymManagement.Application.Interfaces.Repositories.Attendances;
using GymManagement.Application.Interfaces.Services.Attendances;

namespace GymManagement.Application.Services.Converters
{
    public static class AttendanceConverter
    {
        public static AttendanceDto ToDto(this AttendanceDao attendance)
        {
            return new AttendanceDto
            {
                Id = attendance.Id,
                CreatedAt = attendance.CreatedAt,
                UpdatedAt = attendance.UpdatedAt,
                CheckInTime = attendance.CheckInTime
            };
        }

        public static List<AttendanceDto> ToDto(this IEnumerable<AttendanceDao> attendances)
        {
            return attendances.Select(ToDto).ToList();
        }
    }
}

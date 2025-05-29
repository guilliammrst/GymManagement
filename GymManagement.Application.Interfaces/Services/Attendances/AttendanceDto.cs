using GymManagement.Shared.Core.BaseObjects;

namespace GymManagement.Application.Interfaces.Services.Attendances
{
    public class AttendanceDto : BaseDto
    {
        public DateTime CheckInTime { get; set; }
    }
}

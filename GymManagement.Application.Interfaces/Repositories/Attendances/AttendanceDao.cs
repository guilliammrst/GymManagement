using GymManagement.Shared.Core.BaseObjects;

namespace GymManagement.Application.Interfaces.Repositories.Attendances
{
    public class AttendanceDao : BaseDao
    {
        public DateTime CheckInTime { get; set; }
    }
}

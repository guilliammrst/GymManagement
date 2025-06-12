using GymManagement.Shared.Core.Enums;

namespace GymManagement.Shared.Web.Core.Controllers
{
    public class ErrorDto(GymFaultType faultType, string message)
    {
        public int FaultCode { get; } = faultType.GetHashCode();
        public string Label { get; } = faultType.ToString();
        public string Message { get; } = message;
        public long Timestamp { get; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }
}

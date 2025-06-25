using GymManagement.Shared.Core.Enums;
using System.Text.Json.Serialization;

namespace GymManagement.Shared.Web.Core.Controllers
{
    public class ErrorDto
    {
        [JsonPropertyName("faultCode")]
        public int FaultCode { get; set; }

        [JsonPropertyName("label")]
        public string Label { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }

        [JsonConstructor]
        public ErrorDto(int faultCode, string label, string message, long timestamp)
        {
            FaultCode = faultCode;
            Label = label;
            Message = message;
            Timestamp = timestamp;
        }

        public ErrorDto(GymFaultType faultType, string message)
        {
            FaultCode = faultType.GetHashCode();
            Label = faultType.ToString();
            Message = message;
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
    }
}

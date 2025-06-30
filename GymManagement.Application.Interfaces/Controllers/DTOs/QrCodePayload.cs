namespace GymManagement.Application.Interfaces.Controllers.DTOs
{
    public class QrCodePayload
    {
        public int UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}

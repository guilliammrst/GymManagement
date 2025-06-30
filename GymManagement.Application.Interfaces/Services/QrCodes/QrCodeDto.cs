namespace GymManagement.Application.Interfaces.Services.QrCodes
{
    public class QrCodeDto
    {
        public int UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}

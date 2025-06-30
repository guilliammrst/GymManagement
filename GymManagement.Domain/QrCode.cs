using GymManagement.Shared.Core.Enums;
using GymManagement.Shared.Core.Results;
using QRCoder;
using System.Text.Json;

namespace GymManagement.Domain
{
    public class QrCode
    {
        private QrCode(int userId, string token)
        {
            UserId = userId;
            Token = token;
            ExpiresAt = DateTime.UtcNow.AddMinutes(5);
        }

        private QrCode(int userId, string token, DateTime expiresAt)
        {
            UserId = userId;
            Token = token;
            ExpiresAt = expiresAt;
        }

        public int UserId { get; }
        public string Token { get; }
        public DateTime ExpiresAt { get; }

        public static ModelActionResult<QrCode> Create(int userId, string token)
        {
            if (userId <= 0)
                return ModelActionResult<QrCode>.Fail(GymFaultType.BadParameter, "Qr code creation failed: Invalid UserId.");

            if (string.IsNullOrWhiteSpace(token))
                return ModelActionResult<QrCode>.Fail(GymFaultType.BadParameter, "Qr code creation failed: Token cannot be null or empty.");

            return ModelActionResult<QrCode>.Ok(new QrCode(userId, token));
        }

        public static ModelActionResult<QrCode> Build(int userId, string token, DateTime expiresAt)
        {
            if (userId <= 0)
                return ModelActionResult<QrCode>.Fail(GymFaultType.BadParameter, "Qr code creation failed: Invalid UserId.");

            if (string.IsNullOrWhiteSpace(token))
                return ModelActionResult<QrCode>.Fail(GymFaultType.BadParameter, "Qr code creation failed: Token cannot be null or empty.");

            return ModelActionResult<QrCode>.Ok(new QrCode(userId, token, expiresAt));
        }

        public ModelActionResult<string> Generate()
        {
            var json = JsonSerializer.Serialize(this);

            using var qrGenerator = new QRCodeGenerator();
            using var qrData = qrGenerator.CreateQrCode(json, QRCodeGenerator.ECCLevel.Q);
            using var svgQrCode = new SvgQRCode(qrData);
            var svgImage = svgQrCode.GetGraphic(3);

            return ModelActionResult<string>.Ok(svgImage);
        }

        public ModelActionResult Validate()
        {
            if (ExpiresAt < DateTime.UtcNow)
                return ModelActionResult.Fail(GymFaultType.QrCodeExpired, "Qr code validation failed: Qr code has expired.");

            return ModelActionResult.Ok;
        }
    }
}

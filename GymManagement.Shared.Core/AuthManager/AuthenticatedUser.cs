using GymManagement.Shared.Core.Enums;

namespace GymManagement.Shared.Core.AuthManager
{
    public class AuthenticatedUser
    {
        public string Token { get; set; } = string.Empty;
        public DateTime? TokenExpiration { get; set; }
        public string Email { get; set; } = string.Empty;
        public Role Role { get; set; } = Role.None;

        public bool IsAuthenticated => !string.IsNullOrEmpty(Token);
        public bool IsTokenExpired => IsAuthenticated && TokenExpiration != null && TokenExpiration < DateTime.UtcNow;
    }
}

using GymManagement.Core.Enums;

namespace GymManagement.WebApp
{
    public class AuthenticatedUser
    {
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Role Role { get; set; } = Role.None;

        public bool IsAuthenticated => !string.IsNullOrEmpty(Token);
    }
}

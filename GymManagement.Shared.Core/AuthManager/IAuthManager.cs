using GymManagement.Shared.Core.Results;

namespace GymManagement.Shared.Core.AuthManager
{
    public interface IAuthManager
    {
        void RedirectToLogin(bool forceLoad = false);
        public void RedirectToHome();
        Task<ModelActionResult> Login(LoginDto loginDto);
        Task<ModelActionResult> Register(RegisterDto registerDto);
        Task<ModelActionResult> RefreshToken();
        void Logout();
    }
}
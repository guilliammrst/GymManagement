using GymManagement.Shared.Core.Results;

namespace GymManagement.Presentation.WebApp
{
    public interface IAuthManager
    {
        void RedirectToLogin(bool forceLoad = false);
        public void RedirectToHome();
        bool Login(string token);
        Task<ModelActionResult> RefreshToken();
        void Logout();
    }
}
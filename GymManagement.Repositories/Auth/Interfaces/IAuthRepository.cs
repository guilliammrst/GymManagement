using GymManagement.Core.Enums;
using GymManagement.Web.Core.Results;

namespace GymManagement.Repositories.Auth.Interfaces
{
    public interface IAuthRepository
    {
        Task<ModelActionResult<string>> GetUserPasswordByEmail(string? email);
        Task<ModelActionResult<Role>> GetUserRoleByEmail(string? email);
    }
}
using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Interfaces.Repositories.Users
{
    public interface IUserQueryRepository
    {
        Task<ModelActionResult<UserDetailsDao>> GetUserByIdAsync(int id);
        Task<ModelActionResult<UserDetailsDao>> GetUserByEmailAsync(string email);
        Task<ModelActionResult<List<UserDao>>> GetUsersAsync(UserRepositoryFilter filter);
    }
}
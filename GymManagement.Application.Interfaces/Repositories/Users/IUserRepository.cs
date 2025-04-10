using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Interfaces.Repositories.Users
{
    public interface IUserRepository
    {
        Task<ModelActionResult<UserDetailsDao>> GetUserById(int id);
        Task<ModelActionResult<List<UserDao>>> GetUsers();
        Task<ModelActionResult<UserDao>> CreateUser(UserCreateDao userCreate);
    }
}
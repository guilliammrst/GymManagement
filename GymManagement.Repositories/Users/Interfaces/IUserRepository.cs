using GymManagement.Web.Core.Results;

namespace GymManagement.Repositories.Users.Interfaces
{
    public interface IUserRepository
    {
        Task<ModelActionResult<UserDetailsDao>> GetUserById(int id);
        Task<ModelActionResult<List<UserDao>>> GetUsers();
        Task<ModelActionResult<UserDao>> CreateUser(UserCreateDao userCreate);
    }
}
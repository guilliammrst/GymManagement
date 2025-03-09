using GymManagement.Web.Core.Results;

namespace GymManagement.Services.Users.Interfaces
{
    public interface IUserService
    {
        Task<ModelActionResult<UserDto>> GetUserById(int id);
        Task<ModelActionResult<List<UserDto>>> GetUsers();
        Task<ModelActionResult<UserDto>> CreateUser(UserCreateDto userCreateDto);
    }
}

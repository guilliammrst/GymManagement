using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Interfaces.Services.Users
{
    public interface IUserService
    {
        Task<ModelActionResult<UserDto>> GetUserById(int id);
        Task<ModelActionResult<List<UserDto>>> GetUsers();
        Task<ModelActionResult<UserDto>> CreateUser(UserCreateDto userCreateDto);
    }
}

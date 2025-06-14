using GymManagement.Application.Services.Converters;
using GymManagement.Application.Interfaces.Services.Users;
using GymManagement.Shared.Core.Results;
using GymManagement.Application.Interfaces.Repositories.Users;

namespace GymManagement.Application.Services.Users
{
    public class UserQueryService(IUserQueryRepository _userRepository) : IUserQueryService
    {
        public async Task<ModelActionResult<UserDetailsDto>> GetUserByIdAsync(int id)
        {
            var userResult = await _userRepository.GetUserByIdAsync(id);
            if (!userResult.Success)
                return ModelActionResult<UserDetailsDto>.Fail(userResult);

            var user = userResult.Results;

            return ModelActionResult<UserDetailsDto>.Ok(user.ToDetailsDto());
        }

        public async Task<ModelActionResult<List<UserDto>>> GetUsersAsync(UserServiceFilter filter)
        {
            var usersResult = await _userRepository.GetUsersAsync(new UserRepositoryFilter
            {
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                Email = filter.Email
            });
            if (!usersResult.Success)
                return ModelActionResult<List<UserDto>>.Fail(usersResult);

            var users = usersResult.Results;

            return ModelActionResult<List<UserDto>>.Ok(users.ToDto());
        }
    }
}

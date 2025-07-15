using GymManagement.Application.Interfaces.Repositories.Users;
using GymManagement.Application.Interfaces.Services.Users;
using GymManagement.Application.Services.Converters;
using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Services.Users
{
    public class UserQueryService(IUserQueryRepository _userQueryRepository) : IUserQueryService
    {
        public async Task<ModelActionResult<UserDetailsDto>> GetUserByIdAsync(int id)
        {
            var userResult = await _userQueryRepository.GetUserByIdAsync(id);
            if (!userResult.Success)
                return ModelActionResult<UserDetailsDto>.Fail(userResult);

            var user = userResult.Results;

            return ModelActionResult<UserDetailsDto>.Ok(user.ToDetailsDto());
        }

        public async Task<ModelActionResult<List<UserDto>>> GetUsersAsync(UserServiceFilter filter)
        {
            var usersResult = await _userQueryRepository.GetUsersAsync(new UserRepositoryFilter
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

        public async Task<ModelActionResult<UserDetailsDto>> MeAsync(string email)
        {
            var userResult = await _userQueryRepository.GetUserByEmailAsync(email);
            if (!userResult.Success)
                return ModelActionResult<UserDetailsDto>.Fail(userResult);

            var userDao = userResult.Results;
            var userDto = userDao.ToDetailsDto();

            return ModelActionResult<UserDetailsDto>.Ok(userDto);
        }
    }
}

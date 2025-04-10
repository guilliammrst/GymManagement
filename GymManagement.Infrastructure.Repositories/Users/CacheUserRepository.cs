using GymManagement.Shared.Core.CacheManager;
using GymManagement.Shared.Core.Configurations;
using GymManagement.Infrastructure.Repositories.Converters;
using GymManagement.Shared.Core.Results;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using GymManagement.Application.Interfaces.Repositories.Users;

namespace GymManagement.Infrastructure.Repositories.Users
{
    public class CacheUserRepository (IUserRepository _userRepository, IMemoryCache _cache, IOptions<CachingOptions> _cachingOptions) : CacheManager<UserDetailsDao>(_cache, _cachingOptions), IUserRepository
    {
        protected override string CacheKey => CacheKeys.Users;

        public async Task<ModelActionResult<UserDetailsDao>> GetUserByIdAsync(int id)
        {
            var cachedUser = GetCached(id);
            if (cachedUser != null)
                return ModelActionResult<UserDetailsDao>.Ok(cachedUser);

            var userResult = await _userRepository.GetUserByIdAsync(id);
            if (userResult.Success)
                Cache(userResult.Results);

            return userResult;
        }

        public async Task<ModelActionResult<UserDetailsDao>> GetUserByEmailAsync(string email)
        {
            var userResult = await _userRepository.GetUserByEmailAsync(email);
            if (userResult.Success)
                Cache(userResult.Results);

            return userResult;
        }

        public async Task<ModelActionResult<List<UserDao>>> GetUsersAsync()
        {
            return await _userRepository.GetUsersAsync();
        }

        public async Task<ModelActionResult<UserDao>> CreateUserAsync(UserCreateDao userCreate)
        {
            var userResult = await _userRepository.CreateUserAsync(userCreate);
            if (userResult.Success)
                Cache(userResult.Results.ToDetailsDao());

            return userResult;
        }
    }
}

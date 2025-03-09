using GymManagement.Core.CacheManager;
using GymManagement.Core.Configurations;
using GymManagement.Repositories.Converters;
using GymManagement.Repositories.Users.Interfaces;
using GymManagement.Web.Core.Results;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace GymManagement.Repositories.Users
{
    public class CacheUserRepository (IUserRepository _userRepository, IMemoryCache _cache, IOptions<CachingOptions> _cachingOptions) : CacheManager<UserDetailsDao>(_cache, _cachingOptions), IUserRepository
    {
        protected override string CacheKey => CacheKeys.Users;

        public async Task<ModelActionResult<UserDetailsDao>> GetUserById(int id)
        {
            var cachedUser = GetCached(id);
            if (cachedUser != null)
                return ModelActionResult<UserDetailsDao>.Ok(cachedUser);

            var userResult = await _userRepository.GetUserById(id);
            if (userResult.Success)
                Cache(userResult.Results);

            return userResult;
        }

        public async Task<ModelActionResult<List<UserDao>>> GetUsers()
        {
            return await _userRepository.GetUsers();
        }

        public async Task<ModelActionResult<UserDao>> CreateUser(UserCreateDao userCreate)
        {
            var userResult = await _userRepository.CreateUser(userCreate);
            if (userResult.Success)
                Cache(userResult.Results.ToDetailsDao());

            return userResult;
        }
    }
}

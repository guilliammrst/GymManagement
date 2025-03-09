using Dapper;
using GymManagement.Core.Configurations;
using GymManagement.Core.Enums;
using GymManagement.Core.KeyVaultService;
using GymManagement.Repositories.Converters;
using GymManagement.Repositories.DbContexts;
using GymManagement.Repositories.Users.Interfaces;
using GymManagement.Web.Core.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql;

namespace GymManagement.Repositories.Users
{
    public class UserRepository(GymDbContext _context, IKeyVaultService _keyVaultService, IOptions<KeyVaultOptions> options) : IUserRepository
    {
        private readonly KeyVaultOptions _keyVaultSettings = options.Value;

        public async Task<ModelActionResult<UserDetailsDao>> GetUserById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ModelActionResult<List<UserDao>>> GetUsers()
        {
            await using (var connection = new NpgsqlConnection(_keyVaultService.GetValue(_keyVaultSettings.GymDb)))
            {
                const string query = "SELECT * FROM users";
                var users = await connection.QueryAsync<UserDao>(query);
                return ModelActionResult<List<UserDao>>.Ok([.. users]);
            }
        }

        public async Task<ModelActionResult<UserDao>> CreateUser(UserCreateDao userCreate)
        {
            try
            {
                if (_context.Users.All(u => u.Email != userCreate.Email))
                    return ModelActionResult<UserDao>.Fail(GymFaultType.UserAlreadyExists, "User creation failed: user already exists.");

                var userModel = userCreate.ToModel();

                await _context.Users.AddAsync(userModel);
                var result = await _context.SaveChangesAsync();
                if (result == 0)
                    return ModelActionResult<UserDao>.Fail(GymFaultType.UserCreationFailed, "User creation failed: no rows affected.");

                return ModelActionResult<UserDao>.Ok(userModel.ToDao());
            }
            catch (DbUpdateException ex)
            {
                return ModelActionResult<UserDao>.Fail(GymFaultType.DatabaseUnavailable, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
            catch (Exception ex)
            {
                return ModelActionResult<UserDao>.Fail(GymFaultType.UserCreationFailed, ex.Message);
            }
        }
    }
}

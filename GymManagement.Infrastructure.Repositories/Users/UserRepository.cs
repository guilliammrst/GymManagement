using Dapper;
using GymManagement.Shared.Core.Configurations;
using GymManagement.Shared.Core.Enums;
using GymManagement.Infrastructure.Repositories.Converters;
using GymManagement.Infrastructure.Repositories.DbContexts;
using GymManagement.Shared.Core.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql;
using GymManagement.Application.Interfaces.Repositories.Users;

namespace GymManagement.Infrastructure.Repositories.Users
{
    public class UserRepository(GymDbContext _context, IOptions<DbSettings> _options) : IUserRepository
    {
        private readonly string _connectionString = _options.Value.ConnectionString;

        public async Task<ModelActionResult<UserDetailsDao>> GetUserByIdAsync(int id)
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            const string query = "SELECT * FROM users WHERE id = @Id";
            var user = await connection.QueryFirstOrDefaultAsync<UserDetailsDao>(query, new { Id = id });
            if (user == null)
                return ModelActionResult<UserDetailsDao>.Fail(GymFaultType.UserNotFound, "Get user failed: user not found.");
            return ModelActionResult<UserDetailsDao>.Ok(user);
        }

        public async Task<ModelActionResult<UserDetailsDao>> GetUserByEmailAsync(string email)
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            const string query = "SELECT * FROM users WHERE email = @Email";
            var user = await connection.QueryFirstOrDefaultAsync<UserDetailsDao>(query, new { Email = email });
            if (user == null)
                return ModelActionResult<UserDetailsDao>.Fail(GymFaultType.UserNotFound, "Get user failed: user not found.");
            return ModelActionResult<UserDetailsDao>.Ok(user);
        }

        public async Task<ModelActionResult<List<UserDao>>> GetUsersAsync()
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            const string query = "SELECT * FROM users";
            var users = await connection.QueryAsync<UserDao>(query);
            return ModelActionResult<List<UserDao>>.Ok([.. users]);
        }

        public async Task<ModelActionResult<UserDao>> CreateUserAsync(UserCreateDao userCreate)
        {
            try
            {
                if (_context.Users.Any(u => u.Email == userCreate.Email))
                    return ModelActionResult<UserDao>.Fail(GymFaultType.UserEmailAlreadyUsed, "User creation failed: user email already used.");

                if (_context.Users.Any(u => u.PhoneNumber == userCreate.PhoneNumber))
                    return ModelActionResult<UserDao>.Fail(GymFaultType.UserPhoneNumberAlreadyUsed, "User creation failed: user phone number already used.");

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

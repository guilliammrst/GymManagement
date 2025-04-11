using Dapper;
using GymManagement.Shared.Core.Configurations;
using GymManagement.Shared.Core.Enums;
using GymManagement.Application.Interfaces.Repositories.Auth;
using GymManagement.Shared.Core.Results;
using Microsoft.Extensions.Options;
using Npgsql;

namespace GymManagement.Infrastructure.Repositories.Auth
{
    public class AuthRepository(IOptions<DbSettings> _options) : IAuthRepository
    {
        private readonly string _connectionString = _options.Value.ConnectionString;

        public async Task<ModelActionResult<string>> GetUserPasswordByEmailAsync(string? email)
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            const string query = "SELECT password FROM users WHERE email = @Email";
            var userPassword = await connection.QueryFirstOrDefaultAsync<string>(query, new { Email = email });
            if (userPassword == null)
                return ModelActionResult<string>.Fail(GymFaultType.UserNotFound, "User not found.");
            return ModelActionResult<string>.Ok(userPassword);
        }

        public async Task<ModelActionResult<Role>> GetUserRoleByEmailAsync(string? email)
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            const string query = "SELECT role FROM users WHERE email = @Email";
            var userRole = await connection.QueryFirstOrDefaultAsync<Role?>(query, new { Email = email });
            if (userRole == null)
                return ModelActionResult<Role>.Fail(GymFaultType.UserNotFound, "User not found.");
            return ModelActionResult<Role>.Ok((Role)userRole);
        }
    }
}

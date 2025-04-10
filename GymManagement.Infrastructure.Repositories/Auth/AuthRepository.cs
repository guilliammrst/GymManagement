using Dapper;
using GymManagement.Shared.Core.Configurations;
using GymManagement.Shared.Core.Enums;
using GymManagement.Shared.Core.KeyVaultService;
using GymManagement.Application.Interfaces.Repositories.Auth;
using GymManagement.Shared.Core.Results;
using Microsoft.Extensions.Options;
using Npgsql;

namespace GymManagement.Infrastructure.Repositories.Auth
{
    public class AuthRepository (IKeyVaultService _keyVaultService, IOptions<KeyVaultOptions> options) : IAuthRepository
    {
        private readonly KeyVaultOptions _keyVaultOptions = options.Value;

        public async Task<ModelActionResult<string>> GetUserPasswordByEmail(string? email)
        {
            await using (var connection = new NpgsqlConnection(_keyVaultService.GetValue(_keyVaultOptions.GymDb)))
            {
                const string query = "SELECT password FROM users WHERE email = @Email";
                var userPassword = await connection.QueryFirstOrDefaultAsync<string>(query, new { Email = email });
                if (userPassword == null)
                    return ModelActionResult<string>.Fail(GymFaultType.UserNotFound, "User not found.");
                return ModelActionResult<string>.Ok(userPassword);
            }
        }

        public async Task<ModelActionResult<Role>> GetUserRoleByEmail(string? email)
        {
            await using (var connection = new NpgsqlConnection(_keyVaultService.GetValue(_keyVaultOptions.GymDb)))
            {
                const string query = "SELECT role FROM users WHERE email = @Email";
                var userRole = await connection.QueryFirstOrDefaultAsync<Role?>(query, new { Email = email });
                if (userRole == null)
                    return ModelActionResult<Role>.Fail(GymFaultType.UserNotFound, "User not found.");
                return ModelActionResult<Role>.Ok((Role)userRole);
            }
        }
    }
}

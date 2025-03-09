using Dapper;
using GymManagement.Core.Configurations;
using GymManagement.Core.Enums;
using GymManagement.Core.KeyVaultService;
using GymManagement.Repositories.Auth.Interfaces;
using GymManagement.Web.Core.Results;
using Microsoft.Extensions.Options;
using Npgsql;

namespace GymManagement.Repositories.Auth
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

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GymManagement.Core.Configurations;
using GymManagement.Core.Constants;
using GymManagement.Core.Enums;
using GymManagement.Core.KeyVaultService;
using GymManagement.Core.PasswordExt;
using GymManagement.Repositories.Auth.Interfaces;
using GymManagement.Services.Auth.Interfaces;
using GymManagement.Web.Core.Results;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GymManagement.Services.Auth
{
    public class AuthService(IAuthRepository _authRepository, IKeyVaultService _keyVaultService, IOptions<IssuerOptions> options) : IAuthService
    {
        private readonly IssuerOptions _issuerOptions = options.Value;

        public async Task<ModelActionResult> Login(LoginDto loginDto)
        {
            var userPasswordResult = await _authRepository.GetUserPasswordByEmail(loginDto.Email);

            if (!userPasswordResult.Success)
                return ModelActionResult.Fail(GymFaultType.InvalidEmailOrPassword, "Invalid email or password.");

            var userPassword = userPasswordResult.Results;

            if (!loginDto.Password.Verify(userPassword))
                return ModelActionResult.Fail(GymFaultType.InvalidEmailOrPassword, "Invalid email or password.");

            return ModelActionResult.Ok;
        }

        public async Task<ModelActionResult<string>> GenerateToken(string? email)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_keyVaultService.GetValue(_issuerOptions.SecretKey)));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var roleResult = await _authRepository.GetUserRoleByEmail(email);
            if (!roleResult.Success)
                return ModelActionResult<string>.Fail(roleResult);

            var role = roleResult.Results.ToString();

            var claims = new[]
            {
                new Claim(ClaimsTypes.Email, email!),
                new Claim(ClaimsTypes.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimsTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                issuer: _keyVaultService.GetValue(_issuerOptions.Issuer),
                audience: _keyVaultService.GetValue(_issuerOptions.Audience),
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return ModelActionResult<string>.Ok(jwtToken);
        }
    }
}

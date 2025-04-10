using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GymManagement.Shared.Core.Configurations;
using GymManagement.Shared.Core.Constants;
using GymManagement.Shared.Core.Enums;
using GymManagement.Shared.Core.KeyVaultService;
using GymManagement.Shared.Core.PasswordExt;
using GymManagement.Application.Interfaces.Services.Auth;
using GymManagement.Shared.Core.Results;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using GymManagement.Application.Interfaces.Repositories.Auth;

namespace GymManagement.Application.Services.Auth
{
    public class AuthService(IAuthRepository _authRepository, IKeyVaultService _keyVaultService, IOptions<IssuerOptions> options) : IAuthService
    {
        private readonly IssuerOptions _issuerOptions = options.Value;

        public async Task<ModelActionResult<string>> Login(LoginDto loginDto)
        {
            var userPasswordResult = await _authRepository.GetUserPasswordByEmail(loginDto.Email);
            if (!userPasswordResult.Success)
                return ModelActionResult<string>.Fail(GymFaultType.InvalidEmailOrPassword, "Invalid email or password.");

            var userPassword = userPasswordResult.Results;

            if (!loginDto.Password.Verify(userPassword))
                return ModelActionResult<string>.Fail(GymFaultType.InvalidEmailOrPassword, "Invalid email or password.");

            var tokenResult = await GenerateToken(loginDto.Email);
            if (!tokenResult.Success)
                return ModelActionResult<string>.Fail(tokenResult);

            var token = tokenResult.Results;

            return ModelActionResult<string>.Ok(token);
        }

        private async Task<ModelActionResult<string>> GenerateToken(string? email)
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

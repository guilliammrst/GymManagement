using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GymManagement.Shared.Core.Configurations;
using GymManagement.Shared.Core.Constants;
using GymManagement.Shared.Core.Enums;
using GymManagement.Shared.Core.PasswordExt;
using GymManagement.Application.Interfaces.Services.Auth;
using GymManagement.Shared.Core.Results;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using GymManagement.Application.Interfaces.Repositories.Auth;
using GymManagement.Domain;
using GymManagement.Application.Interfaces.Repositories.Users;
using GymManagement.Application.Services.Converters;
using GymManagement.Application.Interfaces.Services.Users;

namespace GymManagement.Application.Services.Auth
{
    public class AuthService(IAuthRepository _authRepository, IUserRepository _userRepository, IOptions<IssuerOptions> options) : IAuthService
    {
        private readonly IssuerOptions _issuerOptions = options.Value;

        public async Task<ModelActionResult<string>> LoginAsync(LoginDto loginDto)
        {
            var userPasswordResult = await _authRepository.GetUserPasswordByEmailAsync(loginDto.Email);
            if (!userPasswordResult.Success)
                return ModelActionResult<string>.Fail(GymFaultType.InvalidEmailOrPassword, "Invalid email or password.");

            var userPassword = userPasswordResult.Results;

            if (!loginDto.Password.Verify(userPassword))
                return ModelActionResult<string>.Fail(GymFaultType.InvalidEmailOrPassword, "Invalid email or password.");

            var tokenResult = await GenerateTokenAsync(loginDto.Email);
            if (!tokenResult.Success)
                return ModelActionResult<string>.Fail(tokenResult);

            var token = tokenResult.Results;

            return ModelActionResult<string>.Ok(token);
        }

        public async Task<ModelActionResult<string>> RegisterAsync(RegisterDto registerDto)
        {
            var userResult = User.Create(registerDto.Name, registerDto.Surname, registerDto.Birthdate, 
                registerDto.Password, Role.None, registerDto.Email, registerDto.PhoneNumber, registerDto.Gender);
            if (!userResult.Success)
                return ModelActionResult<string>.Fail(userResult);

            var user = userResult.Results;
            var userCreateDao = user.ToCreateDao();

            var createUserResult = await _userRepository.CreateUserAsync(userCreateDao);
            if (!createUserResult.Success)
                return ModelActionResult<string>.Fail(createUserResult);

            var userDao = createUserResult.Results;

            var tokenResult = await GenerateTokenAsync(userDao.Email);
            if (!tokenResult.Success)
                return ModelActionResult<string>.Fail(tokenResult);

            var token = tokenResult.Results;

            return ModelActionResult<string>.Ok(token);
        }

        public async Task<ModelActionResult<UserDto>> MeAsync(string email)
        {
            var userResult = await _userRepository.GetUserByEmailAsync(email);
            if (!userResult.Success)
                return ModelActionResult<UserDto>.Fail(userResult);

            var userDao = userResult.Results;
            var userDto = userDao.ToDto();
            
            return ModelActionResult<UserDto>.Ok(userDto);
        }

        private async Task<ModelActionResult<string>> GenerateTokenAsync(string? email)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_issuerOptions.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var roleResult = await _authRepository.GetUserRoleByEmailAsync(email);
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
                issuer: _issuerOptions.Issuer,
                audience: _issuerOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return ModelActionResult<string>.Ok(jwtToken);
        }
    }
}

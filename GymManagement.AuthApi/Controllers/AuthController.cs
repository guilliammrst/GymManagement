using GymManagement.Services.Auth.Interfaces;
using GymManagement.Web.Core.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.AuthApi.Controllers
{
    [ApiController]
    [Route("api/token")]
    public class AuthController (IAuthService _authService) : GymBaseController
    {
        [HttpPost]
        public async Task<ActionResult> GetToken([FromBody] GetTokenDto getTokenDto)
        {
            var loginResult = await _authService.Login(new LoginDto
            {
                Email = getTokenDto.Email,
                Password = getTokenDto.Password
            });
            if (!loginResult.Success)
                return ConvertActionResult(loginResult);

            var tokenResult = await _authService.GenerateToken(getTokenDto.Email);
            if (!tokenResult.Success)
                return ConvertActionResult(tokenResult);

            var token = tokenResult.Results;

            return Ok(token);
        }
    }
}

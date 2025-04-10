using GymManagement.Application.Interfaces.Services.Auth;
using GymManagement.Presentation.AuthApi.DTOs;
using GymManagement.Shared.Web.Core.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Presentation.AuthApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthController (IAuthService _authService) : GymBaseController
    {
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] BodyLoginDto bodyLoginDto)
        {
            var loginResult = await _authService.Login(new LoginDto
            {
                Email = bodyLoginDto.Email,
                Password = bodyLoginDto.Password
            });
            if (!loginResult.Success)
                return ConvertActionResult(loginResult);

            var token = loginResult.Results;

            return Ok(token);
        }
    }
}

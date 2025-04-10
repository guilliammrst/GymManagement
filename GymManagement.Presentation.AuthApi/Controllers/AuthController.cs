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
            var loginResult = await _authService.LoginAsync(new LoginDto
            {
                Email = bodyLoginDto.Email,
                Password = bodyLoginDto.Password
            });
            if (!loginResult.Success)
                return ConvertActionResult(loginResult);

            var token = loginResult.Results;

            return Ok(token);
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] BodyRegisterDto bodyRegisterDto)
        {
            var registerResult = await _authService.RegisterAsync(new RegisterDto
            {
                Email = bodyRegisterDto.Email,
                Password = bodyRegisterDto.Password,
                Name = bodyRegisterDto.Name,
                Surname = bodyRegisterDto.Surname,
                Birthdate = bodyRegisterDto.Birthdate,
                Gender = bodyRegisterDto.Gender,
                PhoneNumber = bodyRegisterDto.PhoneNumber
            });
            if (!registerResult.Success)
                return ConvertActionResult(registerResult);

            var token = registerResult.Results;

            return StatusCode(StatusCodes.Status201Created, token);
        }
    }
}

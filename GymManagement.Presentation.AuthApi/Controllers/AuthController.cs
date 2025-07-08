using GymManagement.Application.Interfaces.Controllers.DTOs;
using GymManagement.Application.Interfaces.Services.Auth;
using GymManagement.Shared.Core.ClaimsHelper;
using GymManagement.Shared.Core.Constants;
using GymManagement.Shared.Web.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Presentation.AuthApi.Controllers
{
    /// <summary>
    /// Controller for user authentication and authorization operations
    /// </summary>
    [ApiController]
    [Route("api")]
    public class AuthController (IAuthService _authService) : GymBaseController
    {
        /// <summary>
        /// Authenticates a user with email and password
        /// </summary>
        /// <param name="bodyLoginDto">Login credentials (email and password)</param>
        /// <returns>JWT token for authenticated access</returns>
        /// <response code="200">Login successful, returns JWT token</response>
        /// <response code="400">Invalid login credentials format</response>
        /// <response code="401">Invalid email or password</response>
        /// <response code="404">User not found</response>
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

        /// <summary>
        /// Refreshes an existing JWT token for continued access
        /// </summary>
        /// <returns>New JWT token</returns>
        /// <response code="200">Token refreshed successfully</response>
        /// <response code="401">Invalid or expired token</response>
        /// <response code="404">User not found</response>
        [HttpGet("refresh-token")]
        [Authorize(Roles = RoleConstants.None + ", " + RoleConstants.Member + ", " + RoleConstants.Coach + ", " + RoleConstants.Staff + ", " + RoleConstants.Manager)]
        public async Task<ActionResult> RefreshToken()
        {
            var emailResult = User.GetEmail();
            if (!emailResult.Success)
                return ConvertActionResult(emailResult);

            var email = emailResult.Results;

            var loginResult = await _authService.RefreshToken(email);
            if (!loginResult.Success)
                return ConvertActionResult(loginResult);

            var token = loginResult.Results;

            return Ok(token);
        }

        /// <summary>
        /// Registers a new user account in the system
        /// </summary>
        /// <param name="bodyRegisterDto">User registration data including personal information</param>
        /// <returns>JWT token for the newly registered user</returns>
        /// <response code="201">User registered successfully, returns JWT token</response>
        /// <response code="400">Invalid registration data</response>
        /// <response code="409">User with this email already exists</response>
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
                PhoneNumber = bodyRegisterDto.PhoneNumber,
                Country = bodyRegisterDto.Country,
                City = bodyRegisterDto.City,
                Street = bodyRegisterDto.Street,
                PostalCode = bodyRegisterDto.PostalCode,
                Number = bodyRegisterDto.Number
            });
            if (!registerResult.Success)
                return ConvertActionResult(registerResult);

            var token = registerResult.Results;

            return StatusCode(StatusCodes.Status201Created, token);
        }

        /// <summary>
        /// Retrieves the authenticated user's profile information
        /// </summary>
        /// <returns>Current user's profile data</returns>
        /// <response code="200">User profile retrieved successfully</response>
        /// <response code="401">Invalid or expired token</response>
        /// <response code="404">User not found</response>
        [HttpGet("me")]
        [Authorize(Roles = RoleConstants.None + ", " + RoleConstants.Member + ", " + RoleConstants.Coach + ", " + RoleConstants.Staff + ", " + RoleConstants.Manager)]
        public async Task<ActionResult> Me()
        {
            var emailResult = User.GetEmail();
            if (!emailResult.Success)
                return ConvertActionResult(emailResult);

            var email = emailResult.Results;

            var userResult = await _authService.MeAsync(email);
            if (!userResult.Success)
                return ConvertActionResult(userResult);

            var user = userResult.Results;
            
            return Ok(user);
        }
    }
}

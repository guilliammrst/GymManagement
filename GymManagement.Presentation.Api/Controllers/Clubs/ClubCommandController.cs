using GymManagement.Application.Interfaces.Controllers.DTOs;
using GymManagement.Application.Interfaces.Services.Clubs;
using GymManagement.Shared.Core.Constants;
using GymManagement.Shared.Web.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Presentation.Api.Controllers.Clubs
{
    [ApiController]
    [Route("api/clubs")]
    [Authorize(Roles = RoleConstants.Staff + ", " + RoleConstants.Manager)]
    public class ClubCommandController(IClubCommandService _clubCommandService) : GymBaseController
    {
        [HttpPost]
        public async Task<ActionResult> CreateClub([FromBody] CreateClubDto clubDto)
        {
            var clubResult = await _clubCommandService.CreateClubAsync(new ClubCreateDto
            {
                Name = clubDto.Name,
                Country = clubDto.Country,
                City = clubDto.City,
                Street = clubDto.Street,
                PostalCode = clubDto.PostalCode,
                Number = clubDto.Number,
                ManagerId = clubDto.ManagerId
            });
            if (!clubResult.Success)
                return ConvertActionResult(clubResult);

            var club = clubResult.Results;

            return new ObjectResult(club)
            {
                StatusCode = StatusCodes.Status201Created
            };
        }

        [HttpPatch("{clubId}")]
        public async Task<ActionResult> UpdateClub([FromRoute] int clubId, [FromBody] UpdateClubDto clubDto)
        {
            var clubResult = await _clubCommandService.UpdateClubAsync(new ClubUpdateDto
            {
                Id = clubId,
                Name = clubDto.Name,
                Country = clubDto.Country,
                City = clubDto.City,
                Street = clubDto.Street,
                PostalCode = clubDto.PostalCode,
                Number = clubDto.Number,
                ManagerId = clubDto.ManagerId
            });
            if (!clubResult.Success)
                return ConvertActionResult(clubResult);

            return NoContent();
        }
    }
}
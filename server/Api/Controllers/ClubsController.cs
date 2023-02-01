using Api.ActionFilters;
using BLLAbstractions;
using Core.DataTransferObjects;
using Core.DataTransferObjects.Club;
using Core.RequestFeatures;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/clubs")]
    public class ClubsController : ControllerBase
    {
        private readonly IClubsService _clubsService;

        public ClubsController(IClubsService clubsService)
        {
            _clubsService = clubsService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetClubs([FromQuery] ClubParameters clubParameters)
        {
            var clubs = await _clubsService.GetClubs(clubParameters);

            return Ok(clubs);
        }

        [HttpGet("{clubId}", Name = "GetSingleClub")]
        public async Task<IActionResult> GetSingleClub(int clubId)
        {
            var club = await _clubsService.GetSingleClub(clubId);

            return Ok(club);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateClub([FromBody] CreateClubDto createClubDto)
        {
            var createdClub = await _clubsService.CreateClub(createClubDto);

            return CreatedAtRoute(
                "GetSingleClub",
                new { clubID = createdClub.ClubId },
                createdClub);
        }

        [HttpDelete("{clubId}")]
        public async Task<IActionResult> DeleteClub(int clubId)
        {
            await _clubsService.DeleteClub(clubId);

            return NoContent();
        }

        [HttpPut("{clubId}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateClub(int clubId, [FromBody] CreateClubDto updateClubDto)
        {
            var updatedClub = await _clubsService
                .UpdateClub(clubId, updateClubDto);

            return Ok(updatedClub);
        }
    }
}
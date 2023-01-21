using BLLAbstractions;
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
        public async Task<IActionResult> GetClubs()
        {
            var clubs = await _clubsService.GetClubs();

            return Ok(clubs.ToList());
        }
    }
}
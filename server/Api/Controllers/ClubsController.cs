using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/clubs")]
    public class ClubsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetClubs()
        {
            return Ok(new {
                id = 1, 
                name = "Білий Ведмідь"
            });
        }
    }
}
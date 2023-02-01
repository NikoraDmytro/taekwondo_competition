using Api.ActionFilters;
using BLLAbstractions;
using Core.DataTransferObjects;
using Core.DataTransferObjects.Sportsman;
using Core.RequestFeatures;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/sportsmans")]
public class SportsmansController : ControllerBase
{
    private readonly ISportsmansService _sportsmansService;

    public SportsmansController(ISportsmansService sportsmansService)
    {
        _sportsmansService = sportsmansService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetSportsmans([FromQuery] SportsmanParameters sportsmanParameters)
    {
        var (sportsmans, pageCount) = await _sportsmansService.GetSportsmans(sportsmanParameters);

        return Ok(new {sportsmans, pageCount});
    }

    [HttpGet("{membershipCardNum}", Name = "GetSingleSportsman")]
    public async Task<IActionResult> GetSingleSportsman(int membershipCardNum)
    {
        var sportsman = await _sportsmansService
            .GetSingleSportsman(membershipCardNum);

        return Ok(sportsman);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateClub([FromBody] CreateSportsmanDto createSportsmanDto)
    {
        var createdSportsman = await _sportsmansService
            .CreateSportsman(createSportsmanDto);

        return CreatedAtRoute(
            "GetSingleSportsman",
            new { membershipCardNum = createdSportsman.MembershipCardNum },
            createdSportsman);
    }

    [HttpDelete("{membershipCardNum}")]
    public async Task<IActionResult> DeleteSportsman(int membershipCardNum)
    {
        await _sportsmansService.DeleteSportsman(membershipCardNum);

        return NoContent();
    }

    [HttpPut("{membershipCardNum}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateSportsman(
        int membershipCardNum, 
        [FromBody] CreateSportsmanDto updateSportsmanDto)
    {
        var updatedSportsman = await _sportsmansService
            .UpdateSportsman(membershipCardNum, updateSportsmanDto);

        return Ok(updatedSportsman);
    }
}

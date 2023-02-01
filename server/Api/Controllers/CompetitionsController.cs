using Api.ActionFilters;
using BLLAbstractions;
using Core.DataTransferObjects.Competition;
using Core.RequestFeatures;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/competitions")]
public class CompetitionsController : ControllerBase
{
    private readonly ICompetitionsService _competitionsService;

    public CompetitionsController(ICompetitionsService competitionsService)
    {
        _competitionsService = competitionsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCompetitions([FromQuery] CompetitionParameters competitionParameters)
    {
        var competitions = await _competitionsService
            .GetCompetitions(competitionParameters);

        return Ok(competitions);
    }

    [HttpGet("{competitionId}", Name = "GetSingleCompetition")]
    public async Task<IActionResult> GetSingleCompetition(int competitionId)
    {
        var competition = await _competitionsService
            .GetSingleCompetition(competitionId);

        return Ok(competition);
    }

    [HttpGet("{competitionId}/sportsmans")]
    public async Task<IActionResult> GetAvailableSportsmans(
        int competitionId,
        [FromQuery] SportsmanParameters sportsmanParameters)
    {
        var (sportsmans, pageCount) = await _competitionsService
            .GetAvailableSportsmans(competitionId, sportsmanParameters);

        return Ok(new
        {
            pageCount,
            sportsmans
        });
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateCompetition([FromBody] CreateCompetitionDto createCompetitionDto)
    {
        var createdCompetition = await _competitionsService
            .CreateCompetition(createCompetitionDto);

        return CreatedAtRoute(
            "GetSingleCompetition",
            new { competitionId = createdCompetition.CompetitionId },
            createdCompetition);
    }

    [HttpDelete("{competitionId}")]
    public async Task<IActionResult> DeleteCompetition(int competitionId)
    {
        await _competitionsService.DeleteCompetition(competitionId);

        return NoContent();
    }

    [HttpPut("{competitionId}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateCompetition(
        int competitionId,
        [FromBody] CreateCompetitionDto updateCompetitionDto)
    {
        var updatedCompetition = await _competitionsService
            .UpdateCompetition(competitionId, updateCompetitionDto);

        return Ok(updatedCompetition);
    }
}
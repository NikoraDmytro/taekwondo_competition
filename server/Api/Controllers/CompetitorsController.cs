using Api.ActionFilters;
using BLLAbstractions;
using Core.DataTransferObjects.Competitor;
using Core.RequestFeatures;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api")]
public class CompetitorsController : ControllerBase
{
    private readonly ICompetitorsService _competitorsService;

    public CompetitorsController(ICompetitorsService competitorsService)
    {
        _competitorsService = competitorsService;
    }

    [HttpGet("competitions/{competitionId}/competitors")]
    public async Task<IActionResult> GetCompetitors(
        int competitionId, 
        [FromQuery] CompetitorParameters competitorParameters)
    {
        var (competitors, pageCount) = await _competitorsService
            .GetCompetitors(competitionId, competitorParameters);

        return Ok(new
        {
            competitors,
            pageCount
        });
    }
    
    [HttpGet("competitors/{applicationNum}", Name = "GetSingleCompetitor")]
    public async Task<IActionResult> GetSingleCompetitor(int applicationNum)
    {
        var competitor = await _competitorsService
            .GetSingleCompetitor(applicationNum);

        return Ok(competitor);
    }

    [HttpPost("competitors")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateCompetitors([FromBody] CreateCompetitorsDto createCompetitorsDto)
    {
        await _competitorsService
            .CreateCompetitor(createCompetitorsDto);

        return NoContent();
    }

    [HttpDelete("competitors/{applicationNum}")]
    public async Task<IActionResult> DeleteCompetitor(int applicationNum)
    {
        await _competitorsService.DeleteCompetitor(applicationNum);

        return NoContent();
    }
}

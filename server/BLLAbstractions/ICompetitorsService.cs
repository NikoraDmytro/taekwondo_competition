using Core.DataTransferObjects.Competitor;
using Core.RequestFeatures;

namespace BLLAbstractions;

public interface ICompetitorsService
{
    Task<(IEnumerable<CompetitorDto>, int)> GetCompetitors(
        int competitionId, 
        CompetitorParameters competitorParameters);
    Task CreateCompetitor(CreateCompetitorsDto createCompetitorDto);
    Task<CompetitorDto> GetSingleCompetitor(int applicationNum);
    Task DeleteCompetitor(int applicationNum);
}
using Core.DataTransferObjects.Competition;
using Core.DataTransferObjects.Sportsman;
using Core.Entities;
using Core.RequestFeatures;

namespace BLLAbstractions;

public interface ICompetitionsService
{
    Task<IEnumerable<Competition>> GetCompetitions(CompetitionParameters competitionParameters);
    Task<Competition> GetSingleCompetition(int competitionId);
    Task<Competition> CreateCompetition(CreateCompetitionDto createCompetitionDto);
    Task DeleteCompetition(int competitionId);
    Task<Competition> UpdateCompetition(int competitionId, CreateCompetitionDto updateCompetitionDto);
    Task<(IEnumerable<SportsmanDto>, int)> GetAvailableSportsmans(int competitionId, SportsmanParameters sportsmanParameters);
}
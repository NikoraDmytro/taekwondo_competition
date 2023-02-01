using Core.DataTransferObjects;
using Core.DataTransferObjects.Sportsman;
using Core.RequestFeatures;

namespace BLLAbstractions;

public interface ISportsmansService
{
    Task<(IEnumerable<SportsmanDto>, int)> GetSportsmans(SportsmanParameters sportsmanParameters);
    Task<SportsmanDto> GetSingleSportsman(int sportsmanId);
    Task<SportsmanDto> CreateSportsman(CreateSportsmanDto createSportsmanDto);
    Task DeleteSportsman(int sportsmanId);
    Task<SportsmanDto> UpdateSportsman(int sportsmanId, CreateSportsmanDto updateSportsmanDto);
}
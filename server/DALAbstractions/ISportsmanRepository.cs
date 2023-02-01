using Core.DataTransferObjects.Sportsman;
using Core.Entities;
using Core.RequestFeatures;

namespace DALAbstractions;

public interface ISportsmanRepository: IGenericRepository<Sportsman>
{
    Task<(IEnumerable<SportsmanDto>, int)> GetSportsmans(SportsmanParameters parameters);
    Task<SportsmanDto?> GetSportsman(object key);
    Task<(IEnumerable<SportsmanDto>, int)> GetSportsmansForCompetition(
        int competitionId,
        SportsmanParameters parameters);
}
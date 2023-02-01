using Core.Entities;
using Core.RequestFeatures;

namespace DALAbstractions;

public interface ICompetitionRepository: IGenericRepository<Competition>
{
    Task<IEnumerable<Competition>> GetCompetitions(CompetitionParameters competitionParameters);
}
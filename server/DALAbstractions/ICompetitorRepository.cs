using Core.Entities;
using Core.RequestFeatures;
using Core.DataTransferObjects.Competitor;

namespace DALAbstractions;

public interface ICompetitorRepository: IGenericRepository<Competitor>
{
    Task<(IEnumerable<CompetitorDto>, int)> GetCompetitors(int competitionId, CompetitorParameters parameters);
    Task<IEnumerable<CompetitorDto>> GetByMembershipCardNums(int competitionId, int[] membershipCardNum);
    Task CreateCompetitors(Competitor[] competitors); 
    Task<CompetitorDto?> GetCompetitor(object key);
}
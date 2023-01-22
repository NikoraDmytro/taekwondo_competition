using Core.Entities;

namespace DALAbstractions;

public interface IClubRepository: IGenericRepository<Club>
{
    Task<IEnumerable<Club>> GetClubs();
}
using Core.Entities;
using Core.RequestFeatures;

namespace DALAbstractions;

public interface IClubRepository: IGenericRepository<Club>
{
    Task<IEnumerable<Club>> GetClubs(ClubParameters parameters);
    Task<Club?> GetClubByName(string name);
}
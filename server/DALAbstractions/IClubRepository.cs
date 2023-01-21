using Core.Entities;

namespace DALAbstractions;

public interface IClubRepository
{
    Task<IEnumerable<Club>> GetClubs();
}
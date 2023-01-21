using Core.Entities;

namespace BLLAbstractions;

public interface IClubsService
{
    Task<IEnumerable<Club>> GetClubs();
}
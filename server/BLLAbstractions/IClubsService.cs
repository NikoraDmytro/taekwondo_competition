using Core.DataTransferObjects;
using Core.DataTransferObjects.Club;
using Core.Entities;
using Core.RequestFeatures;

namespace BLLAbstractions;

public interface IClubsService
{
    Task<IEnumerable<Club>> GetClubs(ClubParameters clubParameters);
    Task<Club> GetSingleClub(int clubId);
    Task<Club> CreateClub(CreateClubDto createClubDto);
    Task DeleteClub(int clubId);
    Task<Club> UpdateClub(int clubId, CreateClubDto updateClubDto);
}
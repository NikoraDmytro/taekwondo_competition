using BLLAbstractions;
using Core.Entities;
using DALAbstractions;

namespace BLL;

public class ClubsService: IClubsService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public ClubsService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<IEnumerable<Club>> GetClubs()
    {
        var clubs = await _unitOfWork.ClubRepository.GetClubs();
        
        return clubs;
    }
}